using CashRegisterSystem.Controllers.Admin.Campaigns;
using CashRegisterSystem.Controllers.Admin.Products;
using CashRegisterSystem.Data;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

public static class AdminMenuController
{
    public static void ShowAdminMenu()
    {
        int adminMenuOption = 1;
        ConsoleKeyInfo key;
        Console.CursorVisible = false;
        string highlight = "\u001b[32m";

        while (true)
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("ADMIN MENY");

            Console.WriteLine($"{(adminMenuOption == 1 ? "·" + highlight : "")} Visa produkter\u001b[0m");
            Console.WriteLine($"{(adminMenuOption == 2 ? "·" + highlight : "")} Ändra produkt\u001b[0m");
            Console.WriteLine($"{(adminMenuOption == 3 ? "·" + highlight : "")} Lägg till produkt\u001b[0m");
            Console.WriteLine($"{(adminMenuOption == 4 ? "·" + highlight : "")} Ta bort produkt\u001b[0m");
            Console.WriteLine($"{(adminMenuOption == 5 ? "·" + highlight : "")} Kampanj meny\u001b[0m");
            Console.WriteLine($"{(adminMenuOption == 6 ? "·" + highlight : "")} Tillbaka till menyn\u001b[0m");

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
                            var products = ProductFileRepository.GetAll();
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
