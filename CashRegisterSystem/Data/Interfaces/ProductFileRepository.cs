using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;
using System.Collections.Generic;

namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Provides simple access to loading and saving all products.
    /// Uses ProductFileLoader as the underlying file handler.
    /// </summary>
    public static class ProductFileRepository
    {
        // Returns all products from file.
        public static List<Product> GetAll()
        {
            var products = ProductFileLoader.GetProducts();
            return products ?? new List<Product>(); // Prevents null crashes
        }

        // Saves all products to file.
        public static void SaveAll(List<Product> products)
        {
            if (products == null)
                return;

            ProductFileLoader.SaveProducts(products);
        }
    }
}
