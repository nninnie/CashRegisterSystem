using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.App
{
    /// <summary>
    //// Entry point of the application. 
    /// Initializes UTF-8 console support for emoji rendering, 
    /// loads all products from file storage, and launches the main menu 
    /// that controls navigation between cashier and admin functionality.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Hämtar alla produkter från filen (eller standardlista om filen saknas)
            List<Product> products = ProductFileLoader.GetProducts();

            // Initierar huvudmenyn och tillhandahåller produktlistan som underlag för menyer och funktioner
            MainMenuView.StartMainMenu(products);
        }
    }
}
