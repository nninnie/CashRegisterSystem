using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Core.Services
{
    public static class ProductService
    {
        public static Product? AddProduct(List<Product> products)
        {
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

            string name = ProductInputUI.AskForName();
            if (string.IsNullOrWhiteSpace(name))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return null;
            }

            decimal price = ProductInputUI.AskForPrice();
            if (price <= 0)
            {
                ProductInputUI.ShowError("Pris måste vara större än 0.");
                return null;
            }

            PriceUnit unit = ProductInputUI.AskForUnit();

            return new Product
            {
                PLU = plu,
                Name = name,
                Price = price,
                PriceUnit = unit
            };
        }

        public static bool DeleteProduct(List<Product> products, string plu)
        {
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
        public static bool EditProduct(List<Product> products, string plu)
        {
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

            string newName = ProductInputUI.AskForName();
            if (string.IsNullOrWhiteSpace(newName))
            {
                ProductInputUI.ShowError("Namn får inte vara tomt.");
                return false;
            }

            decimal newPrice = ProductInputUI.AskForPrice();
            if (newPrice <= 0)
            {
                ProductInputUI.ShowError("Pris måste vara större än 0.");
                return false;
            }

            PriceUnit newUnit = ProductInputUI.AskForUnit();

            // Apply changes
            product.Name = newName;
            product.Price = newPrice;
            product.PriceUnit = newUnit;

            return true;
        }
    }
}
