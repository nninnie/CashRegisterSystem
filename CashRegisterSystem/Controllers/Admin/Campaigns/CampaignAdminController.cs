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
            Console.WriteLine("\nTryck valfri tangent för att återgå...");
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

            string plu = InputService.ReadString("PLU: ");
            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            int option = 1;
            Console.CursorVisible = false;

            const string highlight = "\u001b[32m";
            const string reset = "\u001b[0m";

            Console.WriteLine("\nVälj kampanjtyp med piltangenterna och tryck ENTER:\n");
            int menuTop = Console.CursorTop;

            void DrawMenu()
            {
                Console.SetCursorPosition(0, menuTop);

                for (int i = 0; i < 4; i++)
                {
                    Console.Write(new string(' ', 60));
                    Console.SetCursorPosition(0, menuTop + i);
                }

                Console.SetCursorPosition(0, menuTop);

                Console.WriteLine(option == 1 ? $"· {highlight}Procentkampanj{reset}" : "  Procentkampanj");
                Console.WriteLine(option == 2 ? $"· {highlight}Fast kampanjpris{reset}" : "  Fast kampanjpris");
                Console.WriteLine(option == 3 ? $"· {highlight}Multiköp{reset}" : "  Multiköp");
                Console.WriteLine(option == 4 ? $"· {highlight}Avbryt{reset}" : "  Avbryt");
            }

            DrawMenu();

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow)
                    option = option == 4 ? 1 : option + 1;

                if (key.Key == ConsoleKey.UpArrow)
                    option = option == 1 ? 4 : option - 1;

                DrawMenu();

                if (key.Key == ConsoleKey.Enter)
                    break;
            }

            Console.CursorVisible = true;

            if (option == 4)
                return;

            if (option == 1) ApplyPercentCampaign(product);
            if (option == 2) ApplyFixedPriceCampaign(product);
            if (option == 3) ApplyMultiBuyCampaign(product);

            ProductFileLoader.SaveProducts(products);

            ProductInputUI.ShowConfirmation(
                "Kampanj skapad!",
                $"Produkt: {product.Name}\nTyp: {CampaignService.GetCampaignName(product)}"
            );
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
            decimal percent = InputService.ReadDecimal("Rabatt %: ");
            ResetCampaign(p);
            p.CampaignPercent = percent;
            p.CampaignIsActive = true;
        }

        private static void ApplyFixedPriceCampaign(Product p)
        {
            decimal newPrice = InputService.ReadDecimal("Kampanjpris: ");
            ResetCampaign(p);
            p.CampaignPrice = newPrice;
            p.CampaignIsActive = true;
        }

        private static void ApplyMultiBuyCampaign(Product p)
        {
            int qty = InputService.ReadInt("Antal: ");
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
