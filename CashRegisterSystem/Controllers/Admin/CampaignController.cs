using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin
{
    /// <summary>
    /// Handles user interactions and operations related to campaigns in the cash register system.
    /// </summary>
    /// <remarks>
    /// This controller provides functionality for viewing, creating, editing, and managing campaigns. 
    /// It communicates with <see cref="CampaignRepository"/> for data persistence and ensures that 
    /// campaign-related actions are performed consistently and validated before being saved.
    /// </remarks>
    public class CampaignController
    {
        /// <summary>
        /// Displays the campaign menu in the console and handles user interaction for campaign-related actions.
        /// </summary>
        /// <remarks>This method presents a menu that allows the user to create, view, or end campaigns,
        /// or return to the previous menu. The method blocks until the user makes a selection and then invokes the
        /// corresponding action. The console cursor visibility is managed during menu navigation for a better user
        /// experience.</remarks>
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
                    StartCampaignMenu();
                    break;
                case 2:
                    ViewCampaigns();
                    break;
                case 3:
                    EndCampaign();
                    break;
                case 4:
                    return;
            }

        }


      

      
    }
}
