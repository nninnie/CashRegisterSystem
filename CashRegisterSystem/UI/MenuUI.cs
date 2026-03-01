using CashRegisterSystem.Controllers;
using CashRegisterSystem.Controllers.Admin;
using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class MenuUI
    {
        public static int ShowMainMenu(bool isLoggedIn)
        {
            int selectedMenuOption = 1;
            Console.CursorVisible = false;

            const string highlightColor = "\u001b[32m";

            Console.Clear();
            Console.Write("\x1b[3J");
            Console.SetCursorPosition(0, 0);

            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();

                if (!isLoggedIn)
                {
                    Console.WriteLine($"{(selectedMenuOption == 1 ? "·" + highlightColor : "")} Logga in\u001b[0m");
                    Console.WriteLine($"{(selectedMenuOption == 2 ? "·" + highlightColor : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(selectedMenuOption == 3 ? "·" + highlightColor : "")} Admin\u001b[0m");
                    Console.WriteLine($"{(selectedMenuOption == 4 ? "·" + highlightColor : "")} Avsluta\u001b[0m");
                }
                else
                {
                    Console.WriteLine($"{(selectedMenuOption == 1 ? "·" + highlightColor : "")} Ny kund\u001b[0m");
                    Console.WriteLine($"{(selectedMenuOption == 2 ? "·" + highlightColor : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(selectedMenuOption == 3 ? "·" + highlightColor : "")} Logga ut\u001b[0m");
                }

                ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                if (pressedKey.Key == ConsoleKey.DownArrow)
                    selectedMenuOption++;

                if (pressedKey.Key == ConsoleKey.UpArrow)
                    selectedMenuOption--;

                if (!isLoggedIn)
                {
                    if (selectedMenuOption < 1) selectedMenuOption = 4;
                    if (selectedMenuOption > 4) selectedMenuOption = 1;
                }
                else
                {
                    if (selectedMenuOption < 1) selectedMenuOption = 3;
                    if (selectedMenuOption > 3) selectedMenuOption = 1;
                }

                if (pressedKey.Key == ConsoleKey.Enter)
                {
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    return selectedMenuOption;
                }
            }
        }
    }
}