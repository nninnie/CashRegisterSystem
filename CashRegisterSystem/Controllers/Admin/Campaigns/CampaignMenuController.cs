using CashRegisterSystem.UI;
using System;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    /// <summary>
    /// Displays and handles navigation for the campaign administration menu.
    /// Ensures consistent UI behavior and safe input handling.
    /// </summary>
    public class CampaignMenuController
    {
        public static void ShowCampaignMenu()
        {
            int menuOption = 1;
            ConsoleKeyInfo key;
            Console.CursorVisible = false;

            const string highlight = "\u001b[32m";
            const string reset = "\u001b[0m";

            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();
                BannerUI.PrintHeader("KAMPANJ MENY");

                Console.WriteLine($"{(menuOption == 1 ? "·" + highlight : "  ")} Skapa kampanj{reset}");
                Console.WriteLine($"{(menuOption == 2 ? "·" + highlight : "  ")} Visa kampanjer{reset}");
                Console.WriteLine($"{(menuOption == 3 ? "·" + highlight : "  ")} Avsluta kampanj{reset}");
                Console.WriteLine($"{(menuOption == 4 ? "·" + highlight : "  ")} Tillbaka{reset}");

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
                        Console.CursorVisible = true;

                        switch (menuOption)
                        {
                            case 1:
                                CampaignAdminController.StartCampaign();
                                break;

                            case 2:
                                CampaignAdminController.ViewAllCampaigns();
                                break;

                            case 3:
                                CampaignAdminController.EndCampaign();
                                break;

                            case 4:
                                return;
                        }

                        Console.CursorVisible = false;
                        break;
                }
            }
        }
    }
}