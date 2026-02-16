using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Hämtar alla produkter från filen (eller standardlista om filen saknas)
            List<Product> products = ProductFileLoader.GetProducts();

            // Startar huvudmenyn och skickar med produktlistan
            MainMenuView.StartMainMenu(products);
        }
    }
}
