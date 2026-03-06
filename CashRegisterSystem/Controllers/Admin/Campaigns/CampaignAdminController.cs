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
    /// Responsible only for campaign logic.
    /// </summary>
    public static class CampaignAdminController
    {
        /// <summary>
        /// Displays all active campaigns in a receipt-style layout.
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

            ProductListUI.ShowProductList(products);

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
            ProductListUI.ShowCampaignPLUList(products);

            string? plu = InputService.ReadString("\nAnge PLU för ny kampanj: ");
            if (plu == null)
                return; 

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

            int? choice = InputService.ReadInt("Val: ");
            if (choice == null)
                return; 

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
            ProductListUI.ShowCampaignPLUList(products);

            string? plu = InputService.ReadString("\nAnge PLU på den kampanj du vill ta bort: ");
            if (plu == null)
                return; 

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

        /// <summary>
        /// Starts a percentage discount campaign for the given product.
        /// </summary>
        private static void StartPercentCampaign(Product product)
        {
            decimal? percent = InputService.ReadDecimal("Rabatt i procent: ");
            if (percent == null)
                return; 
            product.CampaignPercent = percent.Value;
            product.CampaignName = "Procentkampanj";
            product.CampaignIsActive = true;
        }

        /// <summary>
        /// Starts a fixed price campaign for the given product.
        /// </summary>
        private static void StartFixedPriceCampaign(Product product)
        {
            decimal? price = InputService.ReadDecimal("Kampanjpris: ");
            if (price == null)
                return;
            product.CampaignPrice = price.Value;
            product.CampaignName = "Fast kampanjpris";
            product.CampaignIsActive = true;
        }

        /// <summary>
        /// Starts a multi-buy campaign for the given product.
        /// </summary>
        private static void StartMultiBuyCampaign(Product product)
        {
            int? quantity = InputService.ReadInt("Antal: ");
            if (quantity == null)
                return;

            decimal? price = InputService.ReadDecimal("Pris för alla: ");
            if (price == null)
                return;

            product.MultiBuyQuantity = quantity.Value;
            product.MultiBuyPrice = price.Value;

            product.CampaignName = "Multiköp";
            product.CampaignIsActive = true;
        }

        /// <summary>
        /// Resets all campaign fields for a product.
        /// </summary>
        private static void ResetCampaign(Product product)
        {
            product.CampaignPercent = 0;
            product.CampaignPrice = 0;
            product.MultiBuyQuantity = 0;
            product.MultiBuyPrice = 0;
            product.CampaignIsActive = false;
            product.CampaignName = null;
        }
    }
}
