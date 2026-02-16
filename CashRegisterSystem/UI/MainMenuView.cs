using CashRegisterSystem.Controllers;
using CashRegisterSystem.Core;

namespace CashRegisterSystem.UI
{
    public static class MainMenuView
    {
        public static void StartMainMenu(List<Product> products)
        {
            // Håller reda på vilket menyval som är markerat
            int mainMenuOption = 1;

            // Läser tangenttryckningar utan att visa dem
            ConsoleKeyInfo key;

            // Dölj markören för snyggare meny
            Console.CursorVisible = false;

            // Grön färg för markerat val
            string highlight = "\u001b[32m";

            // Statusflaggor för kassör och admin
            bool isLoggedIn = false;
            bool isAdmin = false;

            // Huvudloop – menyn visas alltid igen efter ett val
            while (true)
            {
                bool isSelected = false;

                // Menyloop – visas tills användaren trycker Enter
                while (!isSelected)
                {
                    Console.Clear();

                 
                    Console.WriteLine("========================================");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("                 HUVUDMENY");
                    Console.ResetColor();
                    Console.WriteLine("========================================");
                    Console.WriteLine();

                    // ============================
                    // MENY FÖR EJ INLOGGAD ANVÄNDARE
                    // ============================
                    if (!isLoggedIn && !isAdmin)
                    {
                        Console.WriteLine($"{(mainMenuOption == 1 ? "·" + highlight : "")} Logga in\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 2 ? "·" + highlight : "")} PLU-Lista\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 3 ? "·" + highlight : "")} Admin\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 4 ? "·" + highlight : "")} Avsluta\u001b[0m");
                    }

                    // ============================
                    // MENY FÖR INLOGGAD KASSÖR
                    // ============================
                    else if (isLoggedIn)
                    {
                        Console.WriteLine($"{(mainMenuOption == 1 ? "·" + highlight : "")} Starta nytt köp\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 2 ? "·" + highlight : "")} PLU-Lista\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 3 ? "·" + highlight : "")} Logga ut\u001b[0m");
                    }

                    // Läs tangent
                    key = Console.ReadKey(true);

                    // Hantera piltangenter och Enter
                    switch (key.Key)
                    {
                        case ConsoleKey.DownArrow:
                            mainMenuOption++;
                            break;

                        case ConsoleKey.UpArrow:
                            mainMenuOption--;
                            break;

                        case ConsoleKey.Enter:
                            isSelected = true;
                            break;
                    }

                    // Begränsa val beroende på menytyp
                    if (!isLoggedIn && !isAdmin)
                    {
                        if (mainMenuOption < 1) mainMenuOption = 4;
                        if (mainMenuOption > 4) mainMenuOption = 1;
                    }
                    else
                    {
                        if (mainMenuOption < 1) mainMenuOption = 3;
                        if (mainMenuOption > 3) mainMenuOption = 1;
                    }
                }

                // Visa markören igen när vi lämnar menyn
                Console.CursorVisible = true;

                // ============================
                // HANTERA VAL – EJ INLOGGAD
                // ============================
                if (!isLoggedIn && !isAdmin)
                {
                    switch (mainMenuOption)
                    {
                        // ----------------------------
                        // LOGGA IN SOM KASSÖR
                        // ----------------------------
                        case 1:
                            Console.Write("\nKassörsnummer: ");
                            string employeeId = Console.ReadLine()!;

                            // Kontrollera att ID inte är tomt
                            if (!string.IsNullOrWhiteSpace(employeeId))
                            {
                                Console.Write("Ange kassörskod: ");
                                string? code = Console.ReadLine();

                                // Enkel inloggning
                                if (code == "bajskorv".ToLower())
                                {
                                    isLoggedIn = true;
                                    mainMenuOption = 1;
                                }
                                else
                                {
                                    Console.WriteLine("Tyvärr, det är såklart 'bajskorv'!");
                                    Console.ReadKey();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Employee ID får inte vara tomt.");
                                Console.ReadKey();
                            }
                            break;

                        // ----------------------------
                        // VISA PLU-LISTA
                        // ----------------------------
                        case 2:
                            ProductListView.ShowProducts(products);
                            break;

                        // ----------------------------
                        // LOGGA IN SOM ADMIN
                        // ----------------------------
                        case 3:
                            Console.Write("\nAdminlösenord: ");
                            string? adminPass = Console.ReadLine();

                            if (adminPass == "admin".ToLower())
                            {
                                // Hoppa direkt till adminpanelen
                                AdminController.ShowAdminMenu(products);

                                // Tillbaka till huvudmenyn efter adminpanelen
                                isAdmin = false;
                                mainMenuOption = 1;
                            }
                            else
                            {
                                Console.WriteLine("Tyvärr, lösenordet är 'admin'.");
                                Console.ReadKey();
                            }
                            break;

                        // ----------------------------
                        // AVSLUTA PROGRAMMET
                        // ----------------------------
                        case 4:
                            Console.WriteLine("\nTack för idag! Ha en fantastisk dag :)");
                            Environment.Exit(0);
                            break;
                    }
                }

                // ============================
                // HANTERA VAL – KASSÖR INLOGGAD
                // ============================
                else if (isLoggedIn)
                {
                    switch (mainMenuOption)
                    {
                        // Starta nytt köp
                        case 1:
                            CheckoutController.StartNewCustomer(products);
                            break;

                        // Visa PLU-lista
                        case 2:
                            ProductListView.ShowProducts(products);
                            break;

                        // Logga ut kassör
                        case 3:
                            isLoggedIn = false;
                            mainMenuOption = 1;
                            break;
                    }
                }
            }
        }
    }
}
