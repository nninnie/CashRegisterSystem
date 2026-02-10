using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public static class MenuUI
    {
        public static void StartMainMenu(List<ProductModel> products)
        {
            int menuOption = 1;
            bool isSelected = false;
            ConsoleKeyInfo key;

            Console.CursorVisible = false;
            string highlight = "\u001b[32m"; // Grön färg

            while (!isSelected)
            {
                Console.Clear();
                Console.WriteLine("===KASSA===");

                Console.WriteLine($"{(menuOption == 1 ? highlight : "")}> Ny kund\u001b[0m");
                Console.WriteLine($"{(menuOption == 2 ? highlight : "")}> Visa Produkter\u001b[0m");
                Console.WriteLine($"{(menuOption == 3 ? highlight : "")}> Admin\u001b[0m");
                Console.WriteLine($"{(menuOption == 4 ? highlight : "")}> Avsluta programmet\u001b[0m");


                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        menuOption = menuOption == 4 ? 1 : menuOption + 1;
                        break;
                    case ConsoleKey.UpArrow:
                        menuOption = menuOption == 1 ? 4 : menuOption - 1;
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            Console.CursorVisible = true;

            switch (menuOption)
            {
                case 1:
                    CheckoutService.StartNewCustomer(products);
                    break;
                case 2:
                    ProductUI.ShowProducts(products);
                    break;
                case 3:
                    ProductAdminService.ShowAdminMenu(products);
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Tack för ditt besök, välkommemn åter!");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
