using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin
{
    /// <summary>
    /// Provides administrative functionality for managing products, campaigns, 
    /// and other system data within the cash register application.
    /// </summary>
    /// <remarks>
    /// This controller handles menu navigation and user interactions related to 
    /// administrative tasks. It communicates with the appropriate repositories 
    /// to load, modify, and save data, ensuring that changes are consistently 
    /// applied across the system.
    /// </remarks>
    public static class AdminController
    {
        /// <summary>
        /// Displays the administrative menu for managing products and campaigns, allowing the user to select and
        /// perform administrative actions.
        /// </summary>
        /// <remarks>The menu is displayed in the console and requires user interaction via keyboard to
        /// navigate and select options. The method blocks until the user selects an action or chooses to return to the
        /// previous menu.</remarks>
        /// <param name="products">The list of products to be managed. This list is used for editing, adding, or removing products within the
        /// administrative menu.</param>
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
                BannerUI.PrintFruitHeader("ADMIN MENY");

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

