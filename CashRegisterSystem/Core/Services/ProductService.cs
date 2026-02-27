using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Core.Services
{
    public static class ProductService
    {
        public static Product? AddProduct(List<Product> products)
        {
            string plu = ProductUI.AskForPlu();
            if (string.IsNullOrWhiteSpace(plu))
            {
                ProductUI.ShowError("PLU får inte vara tomt.");
                return null;
            }

            if (products.Any(p => p.PLU == plu))
            {
                ProductUI.ShowError("En produkt med detta PLU finns redan.");
                return null;
            }

            string name = ProductUI.AskForName();
            if (string.IsNullOrWhiteSpace(name))
            {
                ProductUI.ShowError("Namn får inte vara tomt.");
                return null;
            }

            decimal price = ProductUI.AskForPrice();
            if (price <= 0)
            {
                ProductUI.ShowError("Pris måste vara större än 0.");
                return null;
            }

            PriceUnit unit = ProductUI.AskForUnit();

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
                ProductUI.ShowError("PLU får inte vara tomt.");
                return false;
            }

            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null)
            {
                ProductUI.ShowError("Ingen produkt hittades med angivet PLU.");
                return false;
            }

            products.Remove(product);
            return true;
        }
        public static bool EditProduct(List<Product> products, string plu)
        {
            if (string.IsNullOrWhiteSpace(plu))
            {
                ProductUI.ShowError("PLU får inte vara tomt.");
                return false;
            }

            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null)
            {
                ProductUI.ShowError("Ingen produkt hittades med angivet PLU.");
                return false;
            }

            string newName = ProductUI.AskForName();
            if (string.IsNullOrWhiteSpace(newName))
            {
                ProductUI.ShowError("Namn får inte vara tomt.");
                return false;
            }

            decimal newPrice = ProductUI.AskForPrice();
            if (newPrice <= 0)
            {
                ProductUI.ShowError("Pris måste vara större än 0.");
                return false;
            }

            PriceUnit newUnit = ProductUI.AskForUnit();

            // Apply changes
            product.Name = newName;
            product.Price = newPrice;
            product.PriceUnit = newUnit;

            return true;
        }
    }
}
