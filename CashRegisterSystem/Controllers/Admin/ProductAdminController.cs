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
    /// Responsible only for product administration 
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

            ProductListUI.ShowCompactPLU(products);

            var newProduct = ProductService.AddProduct(products);

            if (newProduct != null)
            {
                products.Add(newProduct);
                ProductFileRepository.SaveAll(products);

                ProductInputUI.ShowConfirmation(
                    "\nProdukt skapad!",
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

            ProductListUI.ShowCompactPLU(products);

            string? plu = ProductInputUI.AskForPlu();
            if (plu == null)
                return;

            bool removed = ProductService.DeleteProduct(products, plu);

            if (removed)
            {
                ProductFileRepository.SaveAll(products);

                ProductInputUI.ShowConfirmation(
                    "\nProdukt borttagen!",
                    $"PLU: {plu}"
                );
            }

            Console.Clear();
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

            ProductListUI.ShowCompactPLU(products);

            string? plu = ProductInputUI.AskForPlu();
            if (plu == null)
                return;

            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);

            BannerUI.ShowLogo();
            BannerUI.PrintHeader("ÄNDRA PRODUKT");

            ShowProductInfo(product);

            Console.WriteLine();
            Console.WriteLine("Vad vill du ändra?");
            Console.WriteLine("1. PLU");
            Console.WriteLine("2. Namn");
            Console.WriteLine("3. Pris");
            Console.WriteLine("4. Enhet (kg/st)");
            Console.WriteLine("5. Tillbaka");

            int? choice = InputService.ReadInt("Val: ");
            if (choice == null)
                return;

            switch (choice.Value)
            {
                case 1:
                    {
                        string? newPlu = InputService.ReadString("\nNytt PLU: ");
                        if (newPlu == null)
                            return;
                        product.PLU = newPlu;
                        break;
                    }

                case 2:
                    {
                        string? newName = InputService.ReadString("\nNytt namn: ");
                        if (newName == null)
                            return;
                        product.Name = newName;
                        break;
                    }

                case 3:
                    {
                        decimal? newPrice = InputService.ReadDecimal("\nNytt pris: ");
                        if (newPrice == null)
                            return;
                        product.Price = newPrice.Value;
                        break;
                    }

                case 4:
                    {
                        PriceUnit? newUnit = InputService.ReadUnit("\nNy enhet (kg/st): ");
                        if (newUnit == null)
                            return;
                        product.PriceUnit = newUnit.Value;
                        break;
                    }

                case 5:
                    return;

                default:
                    ProductInputUI.ShowError("Ogiltigt val.");
                    return;
            }

            ProductFileRepository.SaveAll(products);

            ProductInputUI.ShowConfirmation(
                "\nProdukten uppdaterad!",
                product.Name
            );

            Console.Clear();
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
    }
}
