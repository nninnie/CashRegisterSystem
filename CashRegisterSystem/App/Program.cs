using CashRegisterSystem.Controllers.Main;
using CashRegisterSystem.Data.FileLoader;

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

            // Skapar products.txt med standardprodukter om filen saknas
            ProductFileLoader.GetProducts();

            MainMenuController.Start();
        }
    }
}
