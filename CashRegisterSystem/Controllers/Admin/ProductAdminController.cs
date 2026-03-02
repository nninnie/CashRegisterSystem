using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data;
using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Controllers.Admin.Products
{
    /// <summary>
    /// Handles admin actions for adding, editing and deleting products.
    /// Responsible only for product administration (no campaign logic).
    /// </summary>
    public static class ProductAdminController
    {
        /// <summary>
        /// Creates a new product using user input and saves it to the product file.
        /// </summary>
        public static void AddProduct()
        {
            var products = ProductFileRepository.GetAll() ?? new List<Product>();

            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("LÄGG TILL PRODUKT");

            var newProduct = ProductService.AddProduct(products);

            if (newProduct != null)
            {
                products.Add(newProduct);
                ProductFileRepository.SaveAll(products);

                ProductInputUI.ShowConfirmation(
                    "Produkt skapad!",
                    $"PLU: {newProduct.PLU}\n" +
                    $"Namn: {newProduct.Name}\n" +
                    $"Pris: {newProduct.Price:0.00} kr"
                );
            }
        }

        /// <summary>
        /// Removes a product based on PLU input and saves the updated list.
        /// </summary>
        public static void DeleteProduct()
        {
            var products = ProductFileRepository.GetAll() ?? new List<Product>();

            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("TA BORT PRODUKT");

            ProductListUI.ShowProductList(products);

            string plu = ProductInputUI.AskForPlu();

            bool removed = ProductService.DeleteProduct(products, plu);

            if (removed)
            {
                ProductFileRepository.SaveAll(products);

                ProductInputUI.ShowConfirmation(
                    "Produkt borttagen!",
                    $"PLU: {plu}"
                );
            }
        }

        /// <summary>
        /// Lets the admin edit a selected product. Shows product info and a clean menu.
        /// </summary>
        public static void EditProduct()
        {
            var products = ProductFileRepository.GetAll() ?? new List<Product>();

            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("ÄNDRA PRODUKT");

            // Use compact PLU list with price (admin version)
            ProductListUI.ShowCompactWithPrice(products);

            string plu = ProductInputUI.AskForPlu();

            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            EditProductMenu(product, products);
        }

        /// <summary>
        /// Shows product info and lets the admin choose what to edit.
        /// </summary>
        private static void EditProductMenu(Product product, List<Product> allProducts)
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("ÄNDRA PRODUKT");

            ShowProductInfo(product);

            Console.WriteLine("\nVälj vad du vill ändra:\n");

            int option = 1;
            int menuTop = Console.CursorTop;

            const string highlight = "\u001b[32m";
            const string reset = "\u001b[0m";

            void DrawMenu()
            {
                Console.SetCursorPosition(0, menuTop);

                for (int i = 0; i < 4; i++)
                {
                    Console.Write(new string(' ', 60));
                    Console.SetCursorPosition(0, menuTop + i);
                }

                Console.SetCursorPosition(0, menuTop);

                Console.WriteLine(option == 1 ? $"· {highlight}Namn{reset}" : "  Namn");
                Console.WriteLine(option == 2 ? $"· {highlight}Pris{reset}" : "  Pris");
                Console.WriteLine(option == 3 ? $"· {highlight}Enhet (kg/st){reset}" : "  Enhet (kg/st)");
                Console.WriteLine(option == 4 ? $"· {highlight}Tillbaka{reset}" : "  Tillbaka");
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

            switch (option)
            {
                case 1:
                    product.Name = InputService.ReadString("Nytt namn: ");
                    break;

                case 2:
                    product.Price = InputService.ReadDecimal("Nytt pris: ");
                    break;

                case 3:
                    product.PriceUnit = InputService.ReadUnit("Ny enhet (kg/st): ");
                    break;

                case 4:
                    return;
            }

            ProductFileRepository.SaveAll(allProducts);

            ProductInputUI.ShowConfirmation(
                "Produkten uppdaterad!",
                product.Name
            );
        }

        /// <summary>
        /// Shows product details in a clean, receipt-like layout.
        /// </summary>
        private static void ShowProductInfo(Product p)
        {
            string unit = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"PLU:        {p.PLU}");
            Console.WriteLine($"Namn:       {p.Name}");
            Console.WriteLine($"Pris:       {p.Price:0} {unit}");

            if (p.CampaignIsActive)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Kampanj:    {CampaignService.GetCampaignName(p)}");
                Console.ResetColor();
            }

            Console.WriteLine("--------------------------------------------------");
        }

        /// <summary>
        /// Displays the full product list in admin mode.
        /// </summary>
        public static void ShowProductListAdmin()
        {
            var products = ProductFileRepository.GetAll() ?? new List<Product>();

            Console.Clear();
            ProductListUI.ShowProductList(products);

            Console.ReadKey(true);
        }
    }
}
