using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public static class ProductAdminService
    {
        public static void ShowAdminMenu(List<ProductModel> products)
        {
            int adminMenuOption = 1;
            bool isSelected = false;
            ConsoleKeyInfo key;

            Console.CursorVisible = false;
            string highlight = "\u001b[32m"; // Grön färg

            while (!isSelected)
            {
                Console.Clear();
                Console.WriteLine("=== ADMIN MENY ===)");

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
                    ProductAdminService.EditProduct(products);
                    break;
                case 2:
                    ProductAdminService.AddProduct(products);
                    break;
                case 3:
                    ProductAdminService.DeleteProduct(products);
                    break;
                case 4:
                    return;
            }
        }

        public static void EditProduct(List<ProductModel> products)
        {
            Console.Clear();
            Console.WriteLine("=== ÄNDRA PRODUKT ===\n"); 
            
            Console.Write("Ange PLU: "); 
            string? plu = Console.ReadLine();

        }

        public static void AddProduct(List<ProductModel> products)
        {
            Console.Clear();
            Console.WriteLine("=== LÄGG TILL PRODUKT ===\n");

        }

        public static void DeleteProduct(List<ProductModel> products)
        {
            Console.Clear();
            Console.WriteLine("=== TA BORT PRODUKT ===\n");

        }

        }
    }
