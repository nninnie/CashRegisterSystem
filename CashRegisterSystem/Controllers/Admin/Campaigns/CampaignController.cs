using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    public class CampaignController
    {
        public static void ShowCampaignMenu()
        {
            int menuOption = 1; // Startar menyn på första alternativet
            bool isSelected = false; // Blir true när användaren trycker Enter och gör sitt val
            ConsoleKeyInfo key; // Lagrar vilken tangent som trycks ned
            Console.CursorVisible = false; // Dölj markören för en renare meny
            string highlight = "\u001b[32m"; // grön markering

            while (!isSelected)
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
                        isSelected = true;
                        break;
                }
            }

            Console.CursorVisible = true;

            switch (menuOption)
            {
                case 1:
                    StartingCampaignMenu.StartCampaignMenu();
                    break;
                case 2:
                    ViewAllCampaigns.ViewCampaigns();
                    break;
                case 3:
                    EndingCampaign.EndCampaign();
                    break;
                case 4:
                    return;
            }
        }
    }
}
