using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Controllers.Admin
{
    /// <summary>
    /// Handles creation, viewing and ending of product campaigns.
    /// Responsible only for campaign logic (no product CRUD).
    /// </summary>
    public static class CampaignAdminController
    {
        /// <summary>
        /// Displays all active campaigns.
        /// </summary>
        public static void ViewAllCampaigns()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("VISA KAMPANJER");

            var products = ProductFileLoader.GetProducts() ?? new List<Product>();

            var campaignProducts = products
                .Where(p => p.CampaignIsActive)
                .ToList();

            if (!campaignProducts.Any())
            {
                ProductInputUI.ShowError("Inga aktiva kampanjer finns.");
                return;
            }

            Console.WriteLine("-----------------------------------------");

            foreach (var p in campaignProducts)
            {
                string type = CampaignService.GetCampaignName(p);

                Console.WriteLine(
                    $"{p.PLU} - {p.Name} | Typ: {type} | " +
                    $"Ord pris: {p.Price:0.00} kr | " +
                    $"Kampanjpris: {CampaignService.GetPrice(p):0.00} kr"
                );
            }

            Console.WriteLine("-----------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nÅtergå till meny..");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        /// <summary>
        /// Starts a campaign for a selected product.
        /// </summary>
            public static void StartCampaign()
            {
                Console.Clear();
                BannerUI.ShowLogo();
                BannerUI.PrintHeader("STARTA KAMPANJ");

                var products = ProductFileLoader.GetProducts() ?? new List<Product>();

                // Visa PLU-lista (admin)
                ProductListUI.ShowCompactWithPrice(products);

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

                // Nollställ gamla kampanjer
                product.CampaignIsActive = false;
                product.CampaignPercent = 0;
                product.CampaignPrice = 0;
                product.MultiBuyQuantity = 0;
                product.MultiBuyPrice = 0;
                product.CampaignName = null;

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

                ProductFileLoader.SaveProducts(products);

                ProductInputUI.ShowConfirmation(
                    "Kampanj skapad!",
                    $"Produkt: {product.Name}\nTyp: {product.CampaignName}"
                );
            }

            private static void StartPercentCampaign(Product product)
            {
                decimal percent = InputService.ReadDecimal("Rabatt i procent: ");

                product.CampaignPercent = percent;
                product.CampaignName = $"{percent}% rabatt";
                product.CampaignIsActive = true;
            }

            private static void StartFixedPriceCampaign(Product product)
            {
                decimal price = InputService.ReadDecimal("Kampanjpris: ");

                product.CampaignPrice = price;
                product.CampaignName = $"Kampanjpris {price:0.00} kr";
                product.CampaignIsActive = true;
            }

            private static void StartMultiBuyCampaign(Product product)
            {
                int qty = InputService.ReadInt("Antal: ");
                decimal price = InputService.ReadDecimal("Pris för alla: ");

                product.MultiBuyQuantity = qty;
                product.MultiBuyPrice = price;
                product.CampaignName = $"{qty} för {price:0.00} kr";
                product.CampaignIsActive = true;
            }
        


        /// <summary>
        /// Ends an active campaign.
        /// </summary>
        public static void EndCampaign()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("AVSLUTA KAMPANJ");

            var products = ProductFileLoader.GetProducts() ?? new List<Product>();

            string plu = InputService.ReadString("PLU: ");
            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            ResetCampaign(product);
            ProductFileLoader.SaveProducts(products);

            ProductInputUI.ShowConfirmation(
                "Kampanj avslutad!",
                $"Produkt: {product.Name}"
            );
        }

        private static void ApplyPercentCampaign(Product p)
        {
            decimal percent = InputService.ReadDecimal("\nRabatt %: ");
            ResetCampaign(p);
            p.CampaignPercent = percent;
            p.CampaignIsActive = true;
        }

        private static void ApplyFixedPriceCampaign(Product p)
        {
            decimal newPrice = InputService.ReadDecimal("\nKampanjpris: ");
            ResetCampaign(p);
            p.CampaignPrice = newPrice;
            p.CampaignIsActive = true;
        }

        private static void ApplyMultiBuyCampaign(Product p)
        {
            int qty = InputService.ReadInt("\nAntal: ");
            decimal price = InputService.ReadDecimal("Paketpris: ");
            ResetCampaign(p);
            p.MultiBuyQuantity = qty;
            p.MultiBuyPrice = price;
            p.CampaignIsActive = true;
        }

        private static void ResetCampaign(Product p)
        {
            p.CampaignPercent = 0;
            p.CampaignPrice = 0;
            p.MultiBuyQuantity = 0;
            p.MultiBuyPrice = 0;
            p.CampaignIsActive = false;
        }
    }
}
