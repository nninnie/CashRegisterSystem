using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data;
using CashRegisterSystem.Data.Interfaces;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin.Products
{
    /// <summary>
    /// Handles admin actions for adding, editing, and deleting products.
    /// </summary>
    public static class ProductController
    {
        /// <summary>
        /// Creates a new product using user input and saves it to the product file.
        /// </summary>
        public static void AddProduct()
        {
            // Hämtar alla produkter från filen
            var products = ProductFileRepository.GetAll();

            // Service-lagret hanterar validering och skapande av produkten
            var newProduct = ProductService.AddProduct(products);

            // Om produkten skapades korrekt, lägg till och spara
            if (newProduct != null)
            {
                products.Add(newProduct);
                ProductFileRepository.SaveAll(products); // Sparar hela listan till fil
            }
        }

        /// <summary>
        /// Removes a product based on PLU input and saves the updated list.
        /// </summary>
        public static void DeleteProduct()
        {
            // Hämtar alla produkter
            var products = ProductFileRepository.GetAll();

            // Visar alla produkter innan borttagning
            ProductUI.ShowProducts(products);

            // Hämtar PLU från användaren (UI ansvarar för input)
            string plu = ProductUI.AskForPlu();

            // Service-lagret hanterar logiken för att ta bort produkten
            bool removed = ProductService.DeleteProduct(products, plu);

            // Om borttagningen lyckades, spara listan
            if (removed)
                ProductFileRepository.SaveAll(products);
        }

        /// <summary>
        /// Updates an existing product using PLU input and saves the changes.
        /// </summary>
        public static void EditProduct()
        {
            // Hämtar alla produkter
            var products = ProductFileRepository.GetAll();

            // Visar produkter innan redigering
            ProductUI.ShowProducts(products);

            // Hämtar PLU från användaren
            string plu = ProductUI.AskForPlu();

            // Service-lagret hanterar redigeringen
            bool updated = ProductService.EditProduct(products, plu);

            // Om uppdateringen lyckades, spara listan
            if (updated)
                ProductFileRepository.SaveAll(products);
        }
    }
}
