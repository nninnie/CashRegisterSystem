using CashRegisterSystem.Controllers.Main;
using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;

namespace CashRegisterSystem.App
{
    /// <summary>
    /// Entry point for the cash register application.
    /// Loads product data and launches the main menu.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load all products from file (fallback to empty list if file missing)
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Load all products from file (fallback to empty list if file missing)
            List<Product> products = ProductFileLoader.GetProducts() ?? new List<Product>();

            // Start main menu with loaded products
            MainMenuController.Start();

        }
    }
}
