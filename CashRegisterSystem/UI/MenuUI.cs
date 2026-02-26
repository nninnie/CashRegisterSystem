using CashRegisterSystem.Controllers;
using CashRegisterSystem.Controllers.Admin;
using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays and manages the main menu for the cash register system, including login handling,
    /// navigation, and access to cashier and admin functions.
    /// </summary>
    public static class MenuUI
    {
        /// <summary>
        /// Starts the interactive main menu loop and routes the user to cashier or admin functions.
        /// </summary>
        /// <param name="products">The list of products available in the system.</param>
        public static void StartMainMenu(List<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            int mainMenuOption = 1;
            ConsoleKeyInfo key;

            Console.CursorVisible = false;

            const string highlight = "\u001b[32m";

            bool isLoggedIn = false;

            while (true)
            {
                bool isSelected = false;

                while (!isSelected)
                {
                    Console.Clear();
                    BannerUI.ShowLogo();

                    if (!isLoggedIn)
                    {
                        Console.WriteLine($"{(mainMenuOption == 1 ? "·" + highlight : "")} Logga in\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 2 ? "·" + highlight : "")} PLU\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 3 ? "·" + highlight : "")} Admin\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 4 ? "·" + highlight : "")} Avsluta\u001b[0m");
                    }
                    else
                    {
                        Console.WriteLine($"{(mainMenuOption == 1 ? "·" + highlight : "")} Ny kund\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 2 ? "·" + highlight : "")} PLU\u001b[0m");
                        Console.WriteLine($"{(mainMenuOption == 3 ? "·" + highlight : "")} Logga ut\u001b[0m");
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

                    if (!isLoggedIn)
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

                // EJ INLOGGAD ANVÄNDARE
                if (!isLoggedIn)
                {
                    switch (mainMenuOption)
                    {
                        case 1:
                            Console.Clear(); 
                            BannerUI.ShowLogo(); 

                            Console.Write("Anställningsnummer: ");
                            string? employeeId = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(employeeId))
                            {
                                Console.WriteLine("Anställningsnummer får inte vara tomt.");
                                Console.ReadKey();
                                break;
                            }

                            Console.Write("Lösenord: ");
                            string? code = Console.ReadLine();

                            if (code?.ToLower() == "" )
                            {
                                LoginService.CurrentUserName = employeeId;
                                isLoggedIn = true;
                                mainMenuOption = 1;
                            }
                            else
                            {
                                Console.WriteLine("Tyvärr, det är såklart 'bajskorv'!");
                                Console.ReadKey();
                            }
                            break;

                        case 2:
                            ProductListView.ShowProductList(products);
                            break;

                        case 3:
                            Console.Clear();
                            BannerUI.ShowLogo();

                            Console.Write("Admin ID: ");
                                string? adminId = Console.ReadLine();
                            if(string.IsNullOrWhiteSpace(adminId))
                            {
                                Console.WriteLine("Admin ID får inte vara tomt.");
                                Console.ReadKey();
                                break;
                            }

                            Console.Write("Lösenord: ");
                            string? adminPass = Console.ReadLine();

                            if (adminPass?.ToLower() == "")
                            {
                                AdminController.ShowAdminMenu(products);
                                mainMenuOption = 1;
                            }
                            else
                            {
                                Console.WriteLine("Tyvärr, lösenordet är 'admin'.");
                                Console.ReadKey();
                            }
                            break;

                        case 4:
                            Console.WriteLine("\n\n\t\t\tLoggar ut...");
                            Thread.Sleep(1200); 
                            Console.WriteLine("\t\t    Ha en fantastisk dag ❤️\n");
                            Thread.Sleep(1200);
                            Environment.Exit(0);
                            break;
                    }
                }

                // INLOGGAD KASSÖR
                else
                {
                    switch (mainMenuOption)
                    {
                        case 1:
                            new CheckoutFlowController().StartNewCustomer(products);
                            break;

                        case 2:
                            ProductListView.ShowProductList(products);
                            break;

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
