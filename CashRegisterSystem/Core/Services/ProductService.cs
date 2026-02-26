using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Core.Services
{
    public static class ProductService
    {
        public static Product? AddProduct(List<Product> products)
        {
            string plu = ProductUI.AskForPlu();
            if (string.IsNullOrWhiteSpace(plu)) return null;

            if (products.Any(p => p.PLU == plu)) return null;

            string name = ProductUI.AskForName();
            if (string.IsNullOrWhiteSpace(name)) return null;

            decimal price = ProductUI.AskForPrice();
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
            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null) return false;

            products.Remove(product);
            return true;
        }

        public static bool EditProduct(List<Product> products, string plu)
        {
            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null) return false;

            string newName = ProductUI.AskForName();
            if (!string.IsNullOrWhiteSpace(newName))
                product.Name = newName;

            decimal newPrice = ProductUI.AskForPrice();
            if (newPrice > 0)
                product.Price = newPrice;

            product.PriceUnit = ProductUI.AskForUnit();

            return true;
        }
    }
}
