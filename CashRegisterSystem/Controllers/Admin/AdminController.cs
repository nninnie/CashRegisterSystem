using CashRegisterSystem.Core;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.Controllers.Admin
{
    public static class AdminController
    {
        /// <summary>
        /// Displays the administrative menu for managing products and processes the user's menu selection.
        /// </summary>
        /// <remarks>The menu allows administrators to edit, add, or delete products, start a campaign, or
        /// return to the main menu. The method interacts with the console and requires user input to navigate and
        /// select options.</remarks>
        /// <param name="products">The list of products to be managed through the administrative menu. Cannot be null.</param>
        public static void ShowAdminMenu(List<Product> products)
        {
            int adminMenuOption = 1;
            bool isSelected = false;
            ConsoleKeyInfo key;
            Console.CursorVisible = false;
            string highlight = "\u001b[32m";

            while (!isSelected)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("                ADMINMENY");
                Console.ResetColor();
                Console.WriteLine("========================================");
                Console.WriteLine();


                Console.WriteLine($"{(adminMenuOption == 1 ? "·" + highlight : "")} Ändra produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 2 ? "·" + highlight : "")} Lägg till produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 3 ? "·" + highlight : "")} Ta bort produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 4 ? "·" + highlight : "")} Starta kampanj\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 5 ? "·" + highlight : "")} Tillbaka till menyn\u001b[0m");

                Console.WriteLine();

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        adminMenuOption = adminMenuOption == 5 ? 1 : adminMenuOption + 1;
                        break;

                    case ConsoleKey.UpArrow:
                        adminMenuOption = adminMenuOption == 1 ? 5 : adminMenuOption - 1;
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
                    ProductController.EditProduct(products);
                    break;

                case 2:
                    ProductController.AddProduct(products);
                    break;

                case 3:
                    ProductController.DeleteProduct(products);
                    break;

                case 4:
                    CampaignController.StartCampaignMenu();
                    break;

                case 5:
                    return;
            }
        }




    }
}

