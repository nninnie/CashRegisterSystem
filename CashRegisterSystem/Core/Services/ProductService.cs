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
        // Creates a new product from user input.
        public static Product? AddProduct(List<Product> products)
        {
            if (products == null)
                return null;

            string plu = ProductInputUI.AskForPlu();
            if (string.IsNullOrWhiteSpace(plu))
            {
                ProductInputUI.ShowError("PLU får inte vara tomt.");
                return null;
            }

            if (products.Any(p => p.PLU == plu))
            {
                ProductInputUI.ShowError("En produkt med detta PLU finns redan.");
                return null;
            }

            string name = ProductInputUI.AskForProductName();
            if (string.IsNullOrWhiteSpace(name))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return null;
            }

            decimal price = ProductInputUI.AskForBasePrice();
            if (price <= 0)
            {
                ProductInputUI.ShowError("Pris måste vara större än 0.");
                return null;
            }

            PriceUnit unit = ProductInputUI.AskForPriceUnit();

            return new Product
            {
                PLU = plu,
                Name = name,
                Price = price,
                PriceUnit = unit,

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

        // Deletes a product by PLU.
        public static bool DeleteProduct(List<Product> products, string plu)
        {
            if (products == null)
                return false;

            if (string.IsNullOrWhiteSpace(plu))
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

        // Edits an existing product.
        public static bool EditProduct(List<Product> products, string plu)
        {
            if (products == null)
                return false;

            if (string.IsNullOrWhiteSpace(plu))
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

            string newName = ProductInputUI.AskForProductName();
            if (string.IsNullOrWhiteSpace(newName))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return false;
            }

            decimal newPrice = ProductInputUI.AskForBasePrice();
            if (newPrice <= 0)
            {
                ProductInputUI.ShowError("Pris måste vara större än 0.");
                return false;
            }

            PriceUnit newUnit = ProductInputUI.AskForPriceUnit();

            // Update product
            product.Name = newName;
            product.Price = newPrice;
            product.PriceUnit = newUnit;

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
