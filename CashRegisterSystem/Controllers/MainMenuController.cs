using CashRegisterSystem.Controllers.Admin;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers
{
    public static class MainMenuController
    {
        public static void Start(List<Product> products)
        {
            bool isLoggedIn = false;

            while (true)
            {
                int choice = MenuUI.ShowMainMenu(isLoggedIn);

                // Töm bufferten så Enter inte triggar samma val igen
                while (Console.KeyAvailable) 
                    Console.ReadKey(true);

                if (!isLoggedIn)
                {
                    switch (choice)
                    {
                        case 1: LoginController.Login(ref isLoggedIn); break;
                        case 2: Console.Clear();  ProductListUI.ShowProductList(products); break;
                        case 3: AdminLoginController.Login(); break;
                        case 4: Console.WriteLine("\n\n\n\t     Tack för idag! Ha en fin dag ❤️\n"); Environment.Exit(0); break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case 1: new CheckoutController().StartNewCustomer(products); break;
                        case 2: ProductListUI.ShowProductList(products); break;
                        case 3: isLoggedIn = false; break;
                    }
                }
            }
        }
    }

}
