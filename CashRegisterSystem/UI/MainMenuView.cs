using CashRegisterSystem.Controllers;
using CashRegisterSystem.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashRegisterSystem.UI
{
    public static class MainMenuView
    {
        public static void StartMainMenu(List<Product> products)
        {
            int mainMenuOption = 1;
            ConsoleKeyInfo key;
            Console.CursorVisible = false;
            string highlight = "\u001b[32m";

            bool isLoggedIn = false;   // Kassörstatus
            bool isAdmin = false;      // Adminstatus

            while (true)
            {
                bool isSelected = false;

                while (!isSelected)
                {
                    Console.Clear();
                    Console.WriteLine("========================================");
                    Console.WriteLine("                 HUVUDMENY");
                    Console.WriteLine("========================================");
                    Console.WriteLine();

                    // Dynamisk meny beroende på inloggning
                    if (!isLoggedIn && !isAdmin)
                    {
                        Console.WriteLine($"{(mainMenuOption == 1 ? highlight : "")}> Logga in\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 2 ? highlight : "")}> PLU-Lista\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 3 ? highlight : "")}> Admin\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 4 ? highlight : "")}> Avsluta\u001b[0m");
                    }
                    else if (isLoggedIn)
                    {
                        Console.WriteLine($"{(mainMenuOption == 1 ? highlight : "")}> Starta nytt köp\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 2 ? highlight : "")}> PLU-Lista\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 3 ? highlight : "")}> Logga ut\u001b[0m");
                    }
                    else if (isAdmin)
                    {
                        Console.WriteLine($"{(mainMenuOption == 1 ? highlight : "")}> Adminpanel\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 2 ? highlight : "")}> PLU-Lista\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 3 ? highlight : "")}> Logga ut\u001b[0m");
                    }

                    key = Console.ReadKey(true);

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

                    // Begränsa val beroende på meny
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

                Console.CursorVisible = true;

                // ============================
                // HUVUDMENY – INTE INLOGGAD
                // ============================
                if (!isLoggedIn && !isAdmin)
                {
                    switch (mainMenuOption)
                    {
                        case 1:
                            Console.Write("\nKassörsnummer: ");
                            string employeeId = Console.ReadLine()!;

                            if (!string.IsNullOrWhiteSpace(employeeId))
                            {
                                Console.Write("Ange kassörskod: ");
                                string? code = Console.ReadLine();

                                if (code == "bajskorv".ToLower()) // enkel inloggning
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

                        case 2:
                            ProductListView.ShowProducts(products);
                            break;

                        case 3:
                            Console.Write("\nAdminlösenord: ");
                            string? adminPass = Console.ReadLine();

                            if (adminPass == "admin".ToLower())
                            {
                                isAdmin = true;
                                mainMenuOption = 1;
                            }
                            else
                            {
                                Console.WriteLine("Tyvärr, lösenordet är 'admin'.");
                                Console.ReadKey();
                            }
                            break;

                        case 4:
                            Console.WriteLine("\nTack för idag!");
                            Environment.Exit(0);
                            break;
                    }
                }

                // ============================
                // KASSÖR INLOGGAD
                // ============================
                else if (isLoggedIn)
                {
                    switch (mainMenuOption)
                    {
                        case 1:
                            CheckoutController.StartNewCustomer(products);
                            break;

                        case 2:
                            ProductListView.ShowProducts(products);
                            break;

                        case 3:
                            isLoggedIn = false;
                            mainMenuOption = 1;
                            break;
                    }
                }

                // ============================
                // ADMIN INLOGGAD
                // ============================
                else if (isAdmin)
                {
                    switch (mainMenuOption)
                    {
                        case 1:
                            AdminController.ShowAdminMenu(products);
                            break;

                        case 2:
                            ProductListView.ShowProducts(products);
                            break;

                        case 3:
                            isAdmin = false;
                            mainMenuOption = 1;
                            break;
                    }
                }
            }
        }
    }
}
