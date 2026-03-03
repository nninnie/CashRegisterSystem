using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Controllers.Admin
{
    /// <summary>
    /// Handles creation, viewing and ending of product campaigns.
    /// Responsible only for campaign logic
    /// </summary>
    public static class CampaignAdminController
    {
        /// <summary>
        /// Displays all active campaigns in a 70-char receipt layout.
        /// </summary>
        public static void ViewAllCampaigns()
        {
            Console.Clear();
            BannerUI.PrintHeader("AKTIVA KAMPANJER");

            var products = ProductFileRepository.GetAll()
                ?.Where(p => p.CampaignIsActive)
                .ToList() ?? new List<Product>();

            if (!products.Any())
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Inga aktiva kampanjer.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            // Visa standard-PLU-listan (70 tecken, samma som överallt)
            ProductListUI.ShowProductList(products);

            Console.WriteLine();
            Console.WriteLine("Tryck valfri tangent för att återgå...");
            Console.ReadKey();
        }

        /// <summary>
        /// Starts a new campaign for a selected product.
        /// </summary>
        public static void StartCampaign()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("STARTA KAMPANJ");

            var products = ProductFileRepository.GetAll() ?? new List<Product>();
            ProductListUI.ShowProductList(products);

            string plu = InputService.ReadString("PLU: ");
            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("STARTA KAMPANJ");

            Console.WriteLine("Välj kampanjtyp:");
            Console.WriteLine("1. Procentkampanj");
            Console.WriteLine("2. Fast kampanjpris");
            Console.WriteLine("3. Multiköp");
            Console.WriteLine("4. Avbryt");

            int choice = InputService.ReadInt("Val: ");

            ResetCampaign(product);

            switch (choice)
            {
                case 1:
                    StartPercentCampaign(product);
                    break;

                case 2:
                    StartFixedPriceCampaign(product);
                    break;

                case 3:
                    StartMultiBuyCampaign(product);
                    break;

                case 4:
                    return;

                default:
                    ProductInputUI.ShowError("Ogiltigt val.");
                    return;
            }

            ProductFileRepository.SaveAll(products);

            ProductInputUI.ShowConfirmation(
                "Kampanj skapad!",
                $"PLU: {product.PLU}\nProdukt: {product.Name}\nTyp: {product.CampaignName}"
            );
        }

        /// <summary>
        /// Ends an active campaign for a product.
        /// </summary>
        public static void EndCampaign()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("AVSLUTA KAMPANJ");

            var products = ProductFileRepository.GetAll() ?? new List<Product>();
            ProductListUI.ShowProductList(products);

            string plu = InputService.ReadString("PLU: ");
            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            ResetCampaign(product);
            ProductFileRepository.SaveAll(products);

            ProductInputUI.ShowConfirmation(
                "Kampanj avslutad!",
                $"PLU: {product.PLU}\nProdukt: {product.Name}"
            );
        }

        // -----------------------------
        // Campaign type implementations
        // -----------------------------

        private static void StartPercentCampaign(Product product)
        {
            decimal percent = InputService.ReadDecimal("Rabatt i procent: ");
            product.CampaignPercent = percent;
            product.CampaignName = "Procentkampanj";
            product.CampaignIsActive = true;
        }

        private static void StartFixedPriceCampaign(Product product)
        {
            decimal price = InputService.ReadDecimal("Kampanjpris: ");
            product.CampaignPrice = price;
            product.CampaignName = "Fast kampanjpris";
            product.CampaignIsActive = true;
        }

        private static void StartMultiBuyCampaign(Product product)
        {
            int qty = InputService.ReadInt("Antal: ");
            decimal price = InputService.ReadDecimal("Pris för alla: ");

            product.MultiBuyQuantity = qty;
            product.MultiBuyPrice = price;
            product.CampaignName = "Multiköp";
            product.CampaignIsActive = true;
        }

        /// <summary>
        /// Resets all campaign fields for a product.
        /// </summary>
        private static void ResetCampaign(Product p)
        {
            p.CampaignPercent = 0;
            p.CampaignPrice = 0;
            p.MultiBuyQuantity = 0;
            p.MultiBuyPrice = 0;
            p.CampaignIsActive = false;
            p.CampaignName = null;
        }

        // -----------------------------
        // Layout helpers (70-char)
        // -----------------------------

        private static string Truncate(string text, int max)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            return text.Length <= max ? text : text.Substring(0, max - 1) + "…";
        }

        /// <summary>
        /// Builds a 70-character campaign line in receipt layout.
        /// </summary>
        private static string BuildCampaignLine(Product p)
        {
            // Kolumnbredder
            string plu = p.PLU.PadRight(6);

            string nameRaw = Truncate(p.Name, 14).PadRight(14);
            string name = nameRaw; // ingen färg här

            string typeRaw =
                p.CampaignPercent > 0 ? $"Procent {p.CampaignPercent}%"
                : p.CampaignPrice > 0 ? "Fast pris"
                : p.MultiBuyQuantity > 0 ? $"{p.MultiBuyQuantity} för {p.MultiBuyPrice:0}"
                : "Kampanj";

            string typePadded = Truncate(typeRaw, 12).PadRight(12);
            string type = $"\u001b[92m{typePadded}\u001b[0m";

            string ordRaw = $"Ord: {p.Price:0.00}".PadRight(14);
            string ord = $"\u001b[90m{ordRaw}\u001b[0m";

            string kampRaw = $"Kamp: {CampaignService.GetPrice(p):0.00}".PadRight(14);
            string kamp = $"\u001b[92m{kampRaw}\u001b[0m";

            return $"{plu}| {name}| {type}| {ord}| {kamp}";
        }
    }
    }
