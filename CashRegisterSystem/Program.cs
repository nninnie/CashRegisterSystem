namespace CashRegisterSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<ProductModel> products = ProductSeeder.GetProducts();
            MenuUI.StartMainMenu(products);
        }
    }
}
