using CashRegisterSystem.Controllers.Admin;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;

namespace CashRegisterSystem.Controllers.Main
{
    /// <summary>
    /// Controls the main menu flow for logged-in and logged-out users.
    /// Ensures safe input handling and prevents repeated key triggers.
    /// </summary>
    public static class MainMenuController
    {
        public static void Start(List<Product> products)
        {
            if (products == null)
                products = new List<Product>();

            bool isLoggedIn = false;

            while (true)
            {
                int choice = MenuUI.ShowMainMenu(isLoggedIn);

                // Clear input buffer so ENTER doesn't auto-trigger next action
                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                if (!isLoggedIn)
                {
                    switch (choice)
                    {
                        case 1:
                            LoginController.Login(ref isLoggedIn);
                            break;

                        case 2:
                            Console.Clear();
                            ProductListUI.ShowProductList(products);
                            break;

                        case 3:
                            AdminLoginController.Login();
                            break;

                        case 4:
                            Console.WriteLine("\n\n\n\t     Tack för idag! Ha en fin dag ❤️\n");
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            new CheckoutController().StartNewCustomer(products);
                            break;

                        case 2:
                            ProductListUI.ShowProductList(products);
                            break;

                        case 3:
                            isLoggedIn = false;
                            break;
                    }
                }
            }
        }
    }
}
