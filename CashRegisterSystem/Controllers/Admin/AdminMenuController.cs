using CashRegisterSystem.Controllers.Admin.Campaigns;
using CashRegisterSystem.Controllers.Admin.Products;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;
using System;

public static class AdminMenuController
{
    /// <summary>
    /// Displays the admin menu and handles navigation and actions.
    /// Ensures safe input handling and consistent UI behavior.
    /// </summary>
    public static void ShowAdminMenu()
    {
        int adminMenuOption = 1;
        ConsoleKeyInfo key;
        Console.CursorVisible = false;

        const string highlight = "\u001b[32m";
        const string reset = "\u001b[0m";

        while (true)
        {
            var products = ProductFileRepository.GetAll();

            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("ADMIN MENY");

            Console.WriteLine($"{(adminMenuOption == 1 ? "·" + highlight : "  ")} Visa produkter{reset}");
            Console.WriteLine($"{(adminMenuOption == 2 ? "·" + highlight : "  ")} Ändra produkt{reset}");
            Console.WriteLine($"{(adminMenuOption == 3 ? "·" + highlight : "  ")} Lägg till produkt{reset}");
            Console.WriteLine($"{(adminMenuOption == 4 ? "·" + highlight : "  ")} Ta bort produkt{reset}");
            Console.WriteLine($"{(adminMenuOption == 5 ? "·" + highlight : "  ")} Kampanj meny{reset}");
            Console.WriteLine($"{(adminMenuOption == 6 ? "·" + highlight : "  ")} Tillbaka till menyn{reset}");

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    adminMenuOption = adminMenuOption == 6 ? 1 : adminMenuOption + 1;
                    break;

                case ConsoleKey.UpArrow:
                    adminMenuOption = adminMenuOption == 1 ? 6 : adminMenuOption - 1;
                    break;

                case ConsoleKey.Enter:
                    Console.CursorVisible = true;

                    switch (adminMenuOption)
                    {
                        case 1:
                            Console.Clear();
                            ProductListUI.ShowProductList(products);
                            break;

                        case 2:
                            Console.Clear();
                            ProductAdminController.EditProduct();
                            break;

                        case 3:
                            Console.Clear();
                            ProductAdminController.AddProduct();
                            break;

                        case 4:
                            Console.Clear();
                            ProductAdminController.DeleteProduct();
                            break;

                        case 5:
                            Console.Clear();
                            CampaignMenuController.ShowCampaignMenu();
                            break;

                        case 6:
                            return;
                    }

                    Console.CursorVisible = false;
                    break;
            }
        }
    }
}
