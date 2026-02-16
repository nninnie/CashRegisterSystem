using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = ProductFileLoader.GetProducts();
            MainMenuView.StartMainMenu(products);
        }
    }
}
