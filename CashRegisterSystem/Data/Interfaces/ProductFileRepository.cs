using CashRegisterSystem.Core;

namespace CashRegisterSystem.Data
{
    public static class ProductFileRepository
    {
        public static List<Product> GetAll()
        {
            return ProductFileLoader.GetProducts();
        }

        public static void SaveAll(List<Product> products)
        {
            ProductFileLoader.SaveProducts(products);
        }
    }
}
