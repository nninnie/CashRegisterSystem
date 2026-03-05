using CashRegisterSystem.Controllers;
using CashRegisterSystem.Controllers.Admin;
using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays the main menu and handles user navigation using arrow keys.
    /// The menu adapts depending on whether the user is logged in or not.
    /// </summary>
    public static class MenuUI
    {
        /// <summary>
        /// Shows the main menu and returns the selected menu option.
        /// </summary>
        public static int ShowMainMenu(bool isLoggedIn)
        {
            int selectedOption = 1;
            Console.CursorVisible = false;

            const string highlightColor = "\u001b[32m";

            Console.Clear();

            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();

                if (!isLoggedIn)
                {
                    BannerUI.PrintHeader("HUVUDMENY");

                    Console.WriteLine($"{(selectedOption == 1 ? "·" + highlightColor : "")} Logga in\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 2 ? "·" + highlightColor : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 3 ? "·" + highlightColor : "")} Admin\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 4 ? "·" + highlightColor : "")} Avsluta\u001b[0m");
                }
                else
                {
                    BannerUI.PrintHeader("KASSÖRINLOGGNING");

                    Console.WriteLine($"{(selectedOption == 1 ? "·" + highlightColor : "")} Ny kund\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 2 ? "·" + highlightColor : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 3 ? "·" + highlightColor : "")} Logga ut\u001b[0m");
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow)
                    selectedOption++;

                if (key.Key == ConsoleKey.UpArrow)
                    selectedOption--;

                if (!isLoggedIn)
                {
                    if (selectedOption < 1) selectedOption = 4;
                    if (selectedOption > 4) selectedOption = 1;
                }
                else
                {
                    if (selectedOption < 1) selectedOption = 3;
                    if (selectedOption > 3) selectedOption = 1;
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    return selectedOption;
                }
            }
        }
    }
}
