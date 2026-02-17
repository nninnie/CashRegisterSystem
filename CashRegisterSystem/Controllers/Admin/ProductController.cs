using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers.Admin
{
    /// <summary>
    /// Handles administrative operations related to products in the cash register system.
    /// </summary>
    /// <remarks>
    /// This controller manages user interactions for viewing, creating, editing, and removing products. 
    /// It communicates with the product repository to load and persist product data, ensuring that 
    /// changes are applied consistently across the system.
    /// </remarks>
    public class ProductController
    {

        /// <summary>
        /// Allows the user to edit the details of an existing product by specifying its PLU code. Updates the provided
        /// product list to reflect any changes made.
        /// </summary>
        /// <remarks>This method interacts with the console to prompt the user for input and displays
        /// product information. The changes are persisted to storage and the provided product list is synchronized with
        /// the updated data. The method requires user interaction and is intended for use in console
        /// applications.</remarks>
        /// <param name="products">The list of products to be updated. This list will be cleared and repopulated with the latest product data
        /// after editing.</param>
        public static void EditProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             ÄNDRA PRODUKT");
            Console.WriteLine("========================================");
            Console.WriteLine();

            // Ladda alltid senaste versionen från filen direkt in i listan
            products.Clear();
            products.AddRange(ProductFileLoader.GetProducts());

            PrintProducts(products);

            Console.Write("\nAnge PLU: ");
            string? enteredPlu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(enteredPlu))
            {
                Console.WriteLine("PLU får inte vara tomt.");
                Console.ReadKey();
                return;
            }

            // Hitta produkten
            var selectedProduct = products.FirstOrDefault(p => p.PLU == enteredPlu);

            if (selectedProduct == null)
            {
                Console.WriteLine("Ingen produkt hittades.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nVald produkt:");
            Console.WriteLine($"1. Namn: {selectedProduct.Name}");
            Console.WriteLine($"2. Pris: {selectedProduct.Price}");
            Console.WriteLine($"3. Enhet: {selectedProduct.PriceUnit}");
            Console.WriteLine("\nTryck Enter för att behålla nuvarande värde.");

            Console.Write("Ändra namn: ");
            string? newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                selectedProduct.Name = newName;

            Console.Write("Ändra pris: ");
            string? newPriceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPriceInput) &&
                decimal.TryParse(newPriceInput, out decimal newPrice))
                selectedProduct.Price = newPrice;

            Console.WriteLine("Välj ny enhet:");
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val: ");
            string? unitChoice = Console.ReadLine();

            if (unitChoice == "1") selectedProduct.PriceUnit = PriceUnit.PerKilo;
            if (unitChoice == "2") selectedProduct.PriceUnit = PriceUnit.Each;

            // Spara tillbaka till filen
            ProductFileLoader.SaveProducts(products);

            Console.WriteLine("\nProdukten har uppdaterats!");
            Console.ReadKey();
        }

        /// <summary>
        /// Adds a new product to the specified product list by prompting the user for product details via the console.
        /// </summary>
        /// <remarks>This method interacts with the user through the console to collect product
        /// information, including PLU, name, price, and unit. It prevents adding a product with a duplicate PLU and
        /// saves the updated product list after addition. The method clears the console and pauses for user input at
        /// various stages.</remarks>
        /// <param name="products">The list of products to which the new product will be added. Cannot be null.</param>
        public static void AddProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             LÄGG TILL PRODUKT");
            Console.WriteLine("========================================");
            Console.WriteLine();


            Console.Write("PLU: ");
            string? plu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(plu))
            {
                Console.WriteLine("PLU får inte vara tomt.");
                Console.ReadKey();
                return;
            }

            if (products.Any(p => p.PLU == plu))
            {
                Console.WriteLine("En produkt med detta PLU finns redan.");
                Console.ReadKey();
                return;
            }

            Console.Write("Namn: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Namn får inte vara tomt.");
                Console.ReadKey();
                return;
            }

            Console.Write("Pris: ");
            string? priceInput = Console.ReadLine();

            if (!decimal.TryParse(priceInput, out decimal price))
            {
                Console.WriteLine("Ogiltigt pris.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enhet:");
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val: ");
            string? unitChoice = Console.ReadLine();

            PriceUnit unit = unitChoice == "1" ? PriceUnit.PerKilo : PriceUnit.Each;

            var newProduct = new Product
            {
                PLU = plu,
                Name = name!,
                Price = price,
                PriceUnit = unit
            };

            products.Add(newProduct);
            ProductFileLoader.SaveProducts(products);

            Console.WriteLine("\nProdukten har lagts till!");
            Console.ReadKey();
        }

        /// <summary>
        /// Removes a product from the specified list based on user input of the product's PLU code.
        /// </summary>
        /// <remarks>The method prompts the user to enter the PLU code of the product to remove. If a
        /// matching product is found, it is removed from the list and the updated list is saved. If no matching product
        /// is found, no changes are made.</remarks>
        /// <param name="products">The list of products from which a product will be removed. Cannot be null.</param>
        public static void DeleteProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             TA BORT PRODUKT");
            Console.WriteLine("========================================");
            Console.WriteLine();


            PrintProducts(products);

            Console.Write("\nAnge PLU att ta bort: ");
            string? plu = Console.ReadLine();

            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                Console.WriteLine("Ingen produkt hittades.");
                Console.ReadKey();
                return;
            }

            products.Remove(product);
            ProductFileLoader.SaveProducts(products);

            Console.WriteLine("\nProdukten har tagits bort!");
            Console.ReadKey();
        }
        /// <summary>
        /// Writes the details of each product in the specified list to the console output.
        /// </summary>
        /// <param name="products">The list of products to display. Each product's PLU, name, and price are written to the console. Cannot be
        /// null.</param>
        public static void PrintProducts(List<Product> products)
        {
            foreach (var p in products)
                Console.WriteLine($"{p.PLU} - {p.Name} - {p.Price} kr");
        }


    }
}
