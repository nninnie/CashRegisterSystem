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

            // Show compact PLU list before creating a new product (same style as other admin views)
            ProductListUI.ShowCompactWithPrice(products);

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

            // Show compact PLU list before asking for PLU
            ProductListUI.ShowCompactWithPrice(products);

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
        /// Lets the admin edit a selected product. Shows product info and a simple menu.
        /// </summary>
        public static void EditProduct()
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("ÄNDRA PRODUKT");

            var products = ProductFileRepository.GetAll() ?? new List<Product>();

            // Show compact PLU list before asking for PLU
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
            BannerUI.PrintHeader("ÄNDRA PRODUKT");

            // Reuse common product info layout (DRY)
            ShowProductInfo(product);

            Console.WriteLine();
            Console.WriteLine("Vad vill du ändra?");
            Console.WriteLine("1. PLU");
            Console.WriteLine("2. Namn");
            Console.WriteLine("3. Pris");
            Console.WriteLine("4. Enhet (kg/st)");
            Console.WriteLine("5. Tillbaka");

            int choice = InputService.ReadInt("Val: ");

            switch (choice)
            {
                case 1:
                    product.PLU = InputService.ReadString("\nNytt PLU: ");
                    break;

                case 2:
                    product.Name = InputService.ReadString("\nNytt namn: ");
                    break;

                case 3:
                    product.Price = InputService.ReadDecimal("\nNytt pris: ");
                    break;

                case 4:
                    product.PriceUnit = InputService.ReadUnit("\nNy enhet (kg/st): ");
                    break;

                case 5:
                    return;

                default:
                    ProductInputUI.ShowError("Ogiltigt val.");
                    return;
            }

            ProductFileRepository.SaveAll(products);

            ProductInputUI.ShowConfirmation(
                "Produkten uppdaterad!",
                product.Name
            );
        }

        /// <summary>
        /// Shows product details in a clean, receipt-like layout.
        /// </summary>
        private static void ShowProductInfo(Product product)
        {
            string unit = product.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"PLU:        {product.PLU}");
            Console.WriteLine($"Namn:       {product.Name}");
            Console.WriteLine($"Pris:       {product.Price:0.00} {unit}");

            if (product.CampaignIsActive)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Kampanj:    {CampaignService.GetCampaignName(product)}");
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
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("PRODUKTLISTA ADMIN");

            ProductListUI.ShowProductList(products);

            Console.ReadKey(true);
        }
    }
}
