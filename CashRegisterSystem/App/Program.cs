using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.App
{
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
