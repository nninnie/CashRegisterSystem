using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    public class CampaignMenuController
    {
        public static void ShowCampaignMenu()
        {
            int menuOption = 1;
            ConsoleKeyInfo key;
            Console.CursorVisible = false;
            string highlight = "\u001b[32m";

            while (true)
            {
                Console.Clear();
                BannerUI.PrintFruitHeader("KAMPANJ-MENY");

                Console.WriteLine($"{(menuOption == 1 ? "·" + highlight : "")} Skapa kampanj\u001b[0m");
                Console.WriteLine($"{(menuOption == 2 ? "·" + highlight : "")} Visa kampanjer\u001b[0m");
                Console.WriteLine($"{(menuOption == 3 ? "·" + highlight : "")} Avsluta kampanj\u001b[0m");
                Console.WriteLine($"{(menuOption == 4 ? "·" + highlight : "")} Tillbaka\u001b[0m");

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
                                Console.Clear();
                                break;

                            case 2:
                                CampaignAdminController.ViewAllCampaigns();
                                Console.Clear();
                                break;

                            case 3:
                                CampaignAdminController.EndCampaign();
                                Console.Clear();
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
