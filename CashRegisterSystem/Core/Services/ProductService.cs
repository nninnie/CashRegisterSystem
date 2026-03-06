using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Core.Services
{
    public static class ProductService
    {
        public static Product? AddProduct(List<Product> products)
        {
            if (products == null)
                return null;

            string? plu = ProductInputUI.AskForPlu(); 

            if (plu == null)
                return null;

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

            string? name = ProductInputUI.AskForProductName();

            if (name == null)
                return null;

            if (string.IsNullOrWhiteSpace(name))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return null;
            }

            decimal? price = ProductInputUI.AskForBasePrice();

            if (price == null)
                return null;

            if (price <= 0)
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
                CampaignPercent = 0,
                CampaignPrice = 0,
                MultiBuyQuantity = 0,
                MultiBuyPrice = 0,
                CampaignIsActive = false,
                CampaignStartDate = null,
                CampaignEndDate = null
            };
        }

        public static bool DeleteProduct(List<Product> products, string? plu)
        {
            if (plu == null)
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

        public static bool EditProduct(List<Product> products, string? plu)
        {
            if (products == null)
                return false;

            if (plu == null)
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

            string? newName = ProductInputUI.AskForProductName();

            if (newName == null)
                return false;

            if (string.IsNullOrWhiteSpace(newName))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return false;
            }

            decimal? newPrice = ProductInputUI.AskForBasePrice();

            if (newPrice == null)
                return false;

            if (newPrice <= 0)
            {
                ProductInputUI.ShowError("Pris måste vara större än 0.");
                return false;
            }

            PriceUnit? newUnit = ProductInputUI.AskForPriceUnit();

            if (newUnit == null)
                return false;

            product.Name = newName;
            product.Price = newPrice.Value;
            product.PriceUnit = newUnit.Value;

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
