using CashRegisterSystem.Controllers.Admin.Campaigns;
using CashRegisterSystem.Controllers.Admin.Products;
using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;


namespace CashRegisterSystem.Controllers.Admin
{
    public static class AdminController
    {
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
                    ProductController.EditProduct();
                    break;

                case 2:
                    ProductController.AddProduct();
                    break;

                case 3:
                    ProductController.DeleteProduct();
                    break;

                case 4:
                    StartingCampaignMenu.StartCampaignMenu();
                    break;

                case 5:
                    return;
            }
        }

    }
}

