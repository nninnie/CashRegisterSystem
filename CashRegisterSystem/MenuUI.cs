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
            while (true)
            {
                int usersMenuInput = MenuUI.ShowMainMenu();
                if (usersMenuInput == -1)
                    continue;

                HandleMainMenuChoice(usersMenuInput, products);
            }
        }

        public static int ShowMainMenu()
        { 
                Console.WriteLine("KASSA");
            Console.WriteLine("1. Ny kund");
            Console.WriteLine("2. Visa Produkter");
            Console.WriteLine("3. Admin");
            Console.WriteLine("4. Avsluta programmet");
            Console.Write("\nMenyval: ");

            string? usersMenuInput = Console.ReadLine();

            if (!int.TryParse(usersMenuInput, out int menuChoice))
                { 
                Console.WriteLine("Fel menyval, försök igen.");
                Console.ReadKey();
                Console.Clear();
                return -1;
            }
            return menuChoice;
        }


        public static void HandleMainMenuChoice(int usersMenuInput, List<ProductModel> products)
        {

            switch (usersMenuInput)
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
                    return;

                default:
                    Console.WriteLine("Fel menyval, försök igen.");
                    break;
            }
        }
    }
}
