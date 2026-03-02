using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;
using CashRegisterSystem.Models;
using System;
using System.Collections.Generic;

namespace CashRegisterSystem.Controllers.Admin.Products
{
    /// <summary>
    /// Handles admin actions for adding, editing and deleting products.
    /// Ensures safe input and consistent UI flow.
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
        /// Updates an existing product using PLU input and saves the changes.
        /// </summary>
        public static void EditProduct()
        {
            var products = ProductFileRepository.GetAll() ?? new List<Product>();

            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("ÄNDRA PRODUKT");
            ProductListUI.ShowProductList(products);

            string plu = ProductInputUI.AskForPlu();

            bool updated = ProductService.EditProduct(products, plu);

            if (updated)
            {
                ProductFileRepository.SaveAll(products);

                ProductInputUI.ShowConfirmation(
                    "Produkt uppdaterad!",
                    $"PLU: {plu}"
                );
            }
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
