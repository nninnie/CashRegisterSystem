using CashRegisterSystem.Controllers.Main;
using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;

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
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Product> products = ProductFileLoader.GetProducts() ?? new List<Product>();

            MainMenuController.Start();
        }
    }
}
