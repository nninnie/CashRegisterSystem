using CashRegisterSystem.Data;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.App
{
    /// <summary>
    /// Entry point for the cash register application, responsible for initializing core data and starting the main menu.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Loads product data and launches the main menu interface for the system.
        /// </summary>
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Hämtar alla produkter från filen (eller standardlista om filen saknas)
            List<Product> products = ProductFileLoader.GetProducts();

            // Initierar huvudmenyn och tillhandahåller produktlistan som underlag för menyer och funktioner
            MainMenuUI.StartMainMenu(products);
        }
    }
}
