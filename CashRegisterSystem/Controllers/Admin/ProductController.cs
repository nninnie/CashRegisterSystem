using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data;
using CashRegisterSystem.Data.Interfaces;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin.Products
{
    public static class ProductController
    {
        public static void AddProduct()
        {
            var products = ProductFileRepository.GetAll();
            var newProduct = ProductService.AddProduct(products);

            if (newProduct != null)
            {
                products.Add(newProduct);
                ProductFileRepository.SaveAll(products);
            }
        }

        public static void DeleteProduct()
        {
            var products = ProductFileRepository.GetAll();
            ProductUI.ShowProducts(products);

            string plu = ProductUI.AskForPlu();
            bool removed = ProductService.DeleteProduct(products, plu);

            if (removed)
                ProductFileRepository.SaveAll(products);
        }

        public static void EditProduct()
        {
            var products = ProductFileRepository.GetAll();
            ProductUI.ShowProducts(products);

            string plu = ProductUI.AskForPlu();
            bool updated = ProductService.EditProduct(products, plu);

            if (updated)
                ProductFileRepository.SaveAll(products);
        }
    }
}
