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

            // Green highlight color for selected menu item
            const string highlightColor = "\u001b[32m";

            Console.Clear();
            Console.Write("\x1b[3J"); // Clears scrollback buffer
            Console.SetCursorPosition(0, 0);

            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();

                // Menu for users who are NOT logged in
                if (!isLoggedIn)
                {
                    Console.WriteLine($"{(selectedOption == 1 ? "·" + highlightColor : "")} Logga in\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 2 ? "·" + highlightColor : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 3 ? "·" + highlightColor : "")} Admin\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 4 ? "·" + highlightColor : "")} Avsluta\u001b[0m");
                }
                else // Menu for logged-in users
                {
                    Console.WriteLine($"{(selectedOption == 1 ? "·" + highlightColor : "")} Ny kund\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 2 ? "·" + highlightColor : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(selectedOption == 3 ? "·" + highlightColor : "")} Logga ut\u001b[0m");
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                // Move selection down
                if (key.Key == ConsoleKey.DownArrow)
                    selectedOption++;

                // Move selection up
                if (key.Key == ConsoleKey.UpArrow)
                    selectedOption--;

                // Wrap menu options depending on login state
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

                // Confirm selection
                if (key.Key == ConsoleKey.Enter)
                {
                    // Clear any buffered key presses
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    return selectedOption;
                }
            }
        }
    }
}
