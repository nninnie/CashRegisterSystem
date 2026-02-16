using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers
{
    public static class AdminController
    {
        public static void ShowAdminMenu(List<Product> products)
        {
            int adminMenuOption = 1;
            bool isSelected = false;
            ConsoleKeyInfo key;

            Console.CursorVisible = false;
            string highlight = "\u001b[32m"; // Grön färg

            while (!isSelected)
            {
                Console.Clear();
                Console.WriteLine("=== ADMIN MENY ===");

                Console.WriteLine($"{(adminMenuOption == 1 ? highlight : "")}> Ändra produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 2 ? highlight : "")}> Lägg till produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 3 ? highlight : "")}> Ta bort produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 4 ? highlight : "")}> Tillbaka\u001b[0m");

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        adminMenuOption = adminMenuOption == 4 ? 1 : adminMenuOption + 1;
                        break;
                    case ConsoleKey.UpArrow:
                        adminMenuOption = adminMenuOption == 1 ? 4 : adminMenuOption - 1;
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            Console.CursorVisible = true;

            switch (adminMenuOption)
            {
                case 1:
                    AdminController.EditProduct(products);
                    break;
                case 2:
                    AdminController.AddProduct(products);
                    break;
                case 3:
                    AdminController.DeleteProduct(products);
                    break;
                case 4:
                    return;
            }
        }

        public static void EditProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== ÄNDRA PRODUKT ===\n");

            // Ladda produkter från fil
            var loadedProducts = ProductFileLoader.GetProducts();

            // Visa lista
            foreach (var p in products) 
            { 
                Console.WriteLine($"{p.PLU} - {p.Name} - {p.Price} kr"); 
            }

            Console.Write("\nAnge PLU: "); 
            string? plu = Console.ReadLine();

            // Validera input
            if(string.IsNullOrWhiteSpace(plu))
            {
                Console.WriteLine("PLU får inte vara tomt.");
                return;
            }

            //Hitta produkten
            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                Console.WriteLine("Ingen produkt hittades med det PLU-numret."); 
                return;
            }

            // Visa nuvarande värden på produkten
            Console.WriteLine($"\nVald produkt:"); 
            Console.WriteLine($"1. Namn: {product.Name}"); 
            Console.WriteLine($"2. Pris: {product.Price}"); 
            Console.WriteLine($"3. Enhet: {product.PriceUnit}"); 
            Console.WriteLine("\nTryck Enter för att behålla nuvarande värde.");

            /// Ändra nammn
            Console.Write("Ändra namn: ");
            string? newNameProduct = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newNameProduct))
            {
                product.Name = newNameProduct;
            }

            // Ändra pris
            Console.Write("Ändra pris: "); 
            string? newPriceProduct = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newPriceProduct) && 
                decimal.TryParse(newPriceProduct, out decimal newPrice))
            {
                product.Price = newPrice;
            }

            // Ändra enhet
            Console.WriteLine("Välj ny enhet:");
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val (Enter för att behålla): ");

            string? unitChoice = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(unitChoice))
            {
                switch (unitChoice)
                {
                    case "1":
                        product.PriceUnit = PriceUnit.PerKilo;
                        break;

                    case "2":
                        product.PriceUnit = PriceUnit.Each;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val. Enheten ändrades inte.");
                        break;
                }
            }


            ProductFileLoader.SaveProducts(products);

            Console.WriteLine($"\nProdukten har uppdateras!");

        }

        public static void AddProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== LÄGG TILL PRODUKT ===\n");

        }

        public static void DeleteProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== TA BORT PRODUKT ===\n");

        }

        }
    }
