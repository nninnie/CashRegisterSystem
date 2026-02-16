using CashRegisterSystem.Controllers;
using CashRegisterSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.UI
{
    public static class MainMenuView
    {
        public static void StartMainMenu(List<Product> products)
        {
            // Håller reda på vilket menyval som är markerat
            int mainMenuOption = 1;

            // Används för att läsa tangenttryckningar utan att visa dem
            ConsoleKeyInfo key;

            // Dölj markören för snyggare meny
            Console.CursorVisible = false;

            // Grön färg för markerat menyval
            string highlight = "\u001b[32m";

            // Huvudloopen – gör att huvudmenyn alltid kommer tillbaka
            while (true)
            {
                // Styr när användaren har valt ett menyval
                bool isSelected = false;

                // Menyloopen – visar menyn tills användaren trycker Enter
                while (!isSelected)
                {
                    Console.Clear();
                    Console.WriteLine("=== HUVUDMENY ===");

                    // Skriver ut menyalternativen och markerar det valda med färg
                    Console.WriteLine($"{(mainMenuOption == 1 ? highlight : "")}> Ny kund\u001b[0m");
                    Console.WriteLine($"{(mainMenuOption == 2 ? highlight : "")}> Visa Produkter\u001b[0m");
                    Console.WriteLine($"{(mainMenuOption == 3 ? highlight : "")}> Admin\u001b[0m");
                    Console.WriteLine($"{(mainMenuOption == 4 ? highlight : "")}> Avsluta \u001b[0m");

                    // Läser tangenttryckning utan att visa den
                    key = Console.ReadKey(true);

                    // Hanterar piltangenter och Enter
                    switch (key.Key)
                    {
                        // Flytta markeringen nedåt, hoppa runt vid slutet
                        case ConsoleKey.DownArrow:
                            mainMenuOption = mainMenuOption == 4 ? 1 : mainMenuOption + 1;
                            break;

                        // Flytta markeringen uppåt, hoppa runt vid början
                        case ConsoleKey.UpArrow:
                            mainMenuOption = mainMenuOption == 1 ? 4 : mainMenuOption - 1;
                            break;

                        // Enter – användaren har valt ett alternativ
                        case ConsoleKey.Enter:
                            isSelected = true;
                            break;
                    }
                }

                // Visa markören igen när vi lämnar menyn
                Console.CursorVisible = true;

                // Kör det valda menyalternativet
                switch (mainMenuOption)
                {
                    case 1:
                        // Startar kassaflödet för en ny kund
                        CheckoutController.StartNewCustomer(products);
                        break;

                    case 2:
                        // Visar alla produkter i systemet
                        ProductListView.ShowProducts(products);
                        break;

                    case 3:
                        // Öppnar adminmenyn
                        AdminController.ShowAdminMenu(products);
                        break;

                    case 4:
                        // Avslutar programmet
                        Console.WriteLine("\nDu loggas nu ut från kassan. Tack för idag och ha en fortsatt fin dag!\n");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
