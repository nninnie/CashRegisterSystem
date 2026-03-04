using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Core.Services
{
    /// <summary>
    /// Handles creation, editing and deletion of products.
    /// Ensures safe input and prevents invalid product data.
    /// </summary>
    public static class ProductService
    {
        /// <summary>
        /// Creates a new product from user input.
        /// Returns null if ESC is pressed or invalid data is entered.
        /// </summary>
        public static Product? AddProduct(List<Product> products)
        {
            if (products == null)
                return null;

            string? plu = ProductInputUI.AskForPlu();
            if (plu == null || string.IsNullOrWhiteSpace(plu))
            {
                ProductInputUI.ShowError("PLU får inte vara tomt.");
                return null;
            }

            if (products.Any(p => p.PLU == plu))
            {
                ProductInputUI.ShowError("En produkt med detta PLU finns redan.");
                return null;
            }

            string? name = ProductInputUI.AskForProductName();
            if (name == null || string.IsNullOrWhiteSpace(name))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return null;
            }

            decimal? price = ProductInputUI.AskForBasePrice();
            if (price == null || price <= 0)
            {
                ProductInputUI.ShowError("Pris måste vara större än 0.");
                return null;
            }

            PriceUnit? unit = ProductInputUI.AskForPriceUnit();
            if (unit == null)
                return null; 

            return new Product
            {
                PLU = plu,
                Name = name,
                Price = price.Value,
                PriceUnit = unit.Value,

                // Reset campaign fields
                CampaignPercent = 0,
                CampaignPrice = 0,
                MultiBuyQuantity = 0,
                MultiBuyPrice = 0,
                CampaignIsActive = false,
                CampaignStartDate = null,
                CampaignEndDate = null
            };
        }

        /// <summary>
        /// Deletes a product by PLU.
        /// Returns false if ESC is pressed or product is not found.
        /// </summary>
        public static bool DeleteProduct(List<Product> products, string? plu)
        {
            if (products == null)
                return false;

            if (plu == null || string.IsNullOrWhiteSpace(plu))
            {
                ProductInputUI.ShowError("PLU får inte vara tomt.");
                return false;
            }

            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades med angivet PLU.");
                return false;
            }

            products.Remove(product);
            return true;
        }

        /// <summary>
        /// Edits an existing product.
        /// Returns false if ESC is pressed or invalid data is entered.
        /// </summary>
        public static bool EditProduct(List<Product> products, string? plu)
        {
            if (products == null)
                return false;

            if (plu == null || string.IsNullOrWhiteSpace(plu))
            {
                ProductInputUI.ShowError("PLU får inte vara tomt.");
                return false;
            }

            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades med angivet PLU.");
                return false;
            }

            string? newName = ProductInputUI.AskForProductName();
            if (newName == null || string.IsNullOrWhiteSpace(newName))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return false;
            }

            decimal? newPrice = ProductInputUI.AskForBasePrice();
            if (newPrice == null || newPrice <= 0)
            {
                ProductInputUI.ShowError("Pris måste vara större än 0.");
                return false;
            }

            PriceUnit? newUnit = ProductInputUI.AskForPriceUnit();
            if (newUnit == null)
                return false;

            // Update product
            product.Name = newName;
            product.Price = newPrice.Value;
            product.PriceUnit = newUnit.Value;

            // Reset campaign data
            product.CampaignPercent = 0;
            product.CampaignPrice = 0;
            product.MultiBuyQuantity = 0;
            product.MultiBuyPrice = 0;
            product.CampaignIsActive = false;
            product.CampaignStartDate = null;
            product.CampaignEndDate = null;

            return true;
        }
    }
}
