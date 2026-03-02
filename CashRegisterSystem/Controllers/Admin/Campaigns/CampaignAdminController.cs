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
    /// Ensures safe input and consistent UI behavior.
    /// </summary>
    public static class CampaignAdminController
    {
        public static void ViewAllCampaigns()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("VISA KAMPANJER");

            var products = ProductFileLoader.GetProducts() ?? new List<Product>();

            var campaignProducts = products
                .Where(p =>
                    p.CampaignIsActive &&
                    (p.CampaignPercent > 0 ||
                     p.CampaignPrice > 0 ||
                     p.MultiBuyQuantity > 0))
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

        public static void StartCampaign()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("STARTA KAMPANJ");

            var products = ProductFileLoader.GetProducts() ?? new List<Product>();
            if (!products.Any())
            {
                ProductInputUI.ShowError("Produktlistan är tom.");
                return;
            }

            ProductListUI.ShowProductList(products);

            string plu = InputService.ReadString("\nPLU: ");
            var selectedProduct = products.FirstOrDefault(p => p.PLU == plu);

            if (selectedProduct == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            int option = 1;
            ConsoleKeyInfo key;
            Console.CursorVisible = false;

            const string highlight = "\u001b[32m";
            const string reset = "\u001b[0m";

            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();
                BannerUI.PrintHeader("VÄLJ KAMPANJTYP");

                Console.WriteLine($"{(option == 1 ? "·" + highlight : "  ")} Procentkampanj{reset}");
                Console.WriteLine($"{(option == 2 ? "·" + highlight : "  ")} Fast kampanjpris{reset}");
                Console.WriteLine($"{(option == 3 ? "·" + highlight : "  ")} Multiköp{reset}");
                Console.WriteLine($"{(option == 4 ? "·" + highlight : "  ")} Avbryt{reset}");

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        option = option == 4 ? 1 : option + 1;
                        break;

                    case ConsoleKey.UpArrow:
                        option = option == 1 ? 4 : option - 1;
                        break;

                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;

                        if (option == 1)
                        {
                            ApplyPercentCampaign(selectedProduct);
                            break;
                        }

                        if (option == 2)
                        {
                            ApplyFixedPriceCampaign(selectedProduct);
                            break;
                        }

                        if (option == 3)
                        {
                            ApplyMultiBuyCampaign(selectedProduct);
                            break;
                        }

                        if (option == 4)
                            return;

                        break;
                }

                if (key.Key == ConsoleKey.Enter)
                    break;
            }

            ProductFileLoader.SaveProducts(products);

            ProductInputUI.ShowConfirmation(
                "Kampanj skapad!",
                $"Produkt: {selectedProduct.Name}\n" +
                $"Typ: {CampaignService.GetCampaignName(selectedProduct)}"
            );
        }

        public static void EndCampaign()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("AVSLUTA KAMPANJ");

            var products = ProductFileLoader.GetProducts() ?? new List<Product>();
            var campaignProducts = products.Where(p => p.CampaignIsActive).ToList();

            if (!campaignProducts.Any())
            {
                ProductInputUI.ShowError("Inga aktiva kampanjer finns.");
                return;
            }

            ProductListUI.ShowProductList(campaignProducts);

            string plu = InputService.ReadString("\nPLU: ");
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
