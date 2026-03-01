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
            int option = 1;
            Console.CursorVisible = false;

            const string highlight = "\u001b[32m";

            Console.Clear();
            Console.Write("\x1b[3J"); // ← Rensar scroll-bufferten i ANSI-terminaler
            Console.SetCursorPosition(0, 0);


            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();

                if (!isLoggedIn)
                {
                    Console.WriteLine($"{(option == 1 ? "·" + highlight : "")} Logga in\u001b[0m");
                    Console.WriteLine($"{(option == 2 ? "·" + highlight : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(option == 3 ? "·" + highlight : "")} Admin\u001b[0m");
                    Console.WriteLine($"{(option == 4 ? "·" + highlight : "")} Avsluta\u001b[0m");
                }
                else
                {
                    Console.WriteLine($"{(option == 1 ? "·" + highlight : "")} Ny kund\u001b[0m");
                    Console.WriteLine($"{(option == 2 ? "·" + highlight : "")} PLU\u001b[0m");
                    Console.WriteLine($"{(option == 3 ? "·" + highlight : "")} Logga ut\u001b[0m");
                }

                // Läs tangenttrycket här (superviktigt!)
                ConsoleKeyInfo key = Console.ReadKey(true);

                // Hantera piltangenter
                if (key.Key == ConsoleKey.DownArrow) option++;
                if (key.Key == ConsoleKey.UpArrow) option--;

                if (!isLoggedIn)
                {
                    if (option < 1) option = 4;
                    if (option > 4) option = 1;
                }
                else
                {
                    if (option < 1) option = 3;
                    if (option > 3) option = 1;
                }

                // Hantera Enter
                if (key.Key == ConsoleKey.Enter)
                {
                    // Töm bufferten så Enter inte registreras två gånger
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    return option;
                }
            }
        }

    }
}
