using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;
using System.Collections.Generic;

namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Provides unified access to loading and saving all products.
    /// Loads products from file and applies campaign data.
    /// Ensures no null values and prevents crashes.
    /// </summary>
    public static class ProductFileRepository
    {
        /// <summary>
        /// Loads all products and applies campaign data.
        /// </summary>
        public static List<Product> GetAll()
        {
            var products = ProductFileLoader.GetProducts() ?? new List<Product>();

            products = CampaignFileLoader.LoadCampaignsIntoProducts(products);

            return products;
        }

        /// <summary>
        /// Saves all products to file.
        /// </summary>
        public static void SaveAll(List<Product> products)
        {
            if (products == null)
                return;

            ProductFileLoader.SaveProducts(products);

            CampaignFileLoader.SaveCampaignsFromProducts(products);
        }
    }
}
