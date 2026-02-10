namespace CashRegisterSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = ProductDataSeeder.GetProducts();
            MainMenuUI.StartMainMenu(products);
        }
    }
}
