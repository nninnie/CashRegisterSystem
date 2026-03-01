using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin
{
    public static class CampaignAdminController
    {
        public static void ViewAllCampaigns()
        {
            Console.Clear();
            BannerUI.PrintHeader("VISA KAMPANJER");

            var products = ProductFileLoader.GetProducts();
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
            BannerUI.PrintHeader("STARTA KAMPANJ");

            var products = ProductFileLoader.GetProducts();
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

            // --- Interaktiv meny ---
            int option = 1;
            ConsoleKeyInfo key;
            Console.CursorVisible = false;
            string highlight = "\u001b[32m";

            while (true)
            {
                Console.Clear();
                BannerUI.PrintHeader("VÄLJ KAMPANJTYP");

                Console.WriteLine($"{(option == 1 ? "·" + highlight : "")} Procentkampanj\u001b[0m");
                Console.WriteLine($"{(option == 2 ? "·" + highlight : "")} Fast kampanjpris\u001b[0m");
                Console.WriteLine($"{(option == 3 ? "·" + highlight : "")} Multiköp\u001b[0m");
                Console.WriteLine($"{(option == 4 ? "·" + highlight : "")} Avbryt\u001b[0m");

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
                            decimal percent = InputService.ReadDecimal("Rabatt %: ");
                            selectedProduct.CampaignPercent = percent;
                            selectedProduct.CampaignPrice = 0;
                            selectedProduct.MultiBuyQuantity = 0;
                            selectedProduct.MultiBuyPrice = 0;
                            selectedProduct.CampaignIsActive = true;
                            break;
                        }

                        if (option == 2)
                        {
                            decimal newPrice = InputService.ReadDecimal("Kampanjpris: ");
                            selectedProduct.CampaignPrice = newPrice;
                            selectedProduct.CampaignPercent = 0;
                            selectedProduct.MultiBuyQuantity = 0;
                            selectedProduct.MultiBuyPrice = 0;
                            selectedProduct.CampaignIsActive = true;
                            break;
                        }

                        if (option == 3)
                        {
                            int qty = InputService.ReadInt("Antal: ");
                            decimal price = InputService.ReadDecimal("Paketpris: ");
                            selectedProduct.MultiBuyQuantity = qty;
                            selectedProduct.MultiBuyPrice = price;
                            selectedProduct.CampaignPercent = 0;
                            selectedProduct.CampaignPrice = 0;
                            selectedProduct.CampaignIsActive = true;
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
            BannerUI.PrintHeader("AVSLUTA KAMPANJ");

            var products = ProductFileLoader.GetProducts();
            var campaignProducts = products
                .Where(p => p.CampaignIsActive)
                .ToList();

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

            product.CampaignIsActive = false;
            product.CampaignPercent = 0;
            product.CampaignPrice = 0;
            product.MultiBuyQuantity = 0;
            product.MultiBuyPrice = 0;

            ProductFileLoader.SaveProducts(products);

            ProductInputUI.ShowConfirmation(
                "Kampanj avslutad!",
                $"Produkt: {product.Name}"
            );
        }
    }
}