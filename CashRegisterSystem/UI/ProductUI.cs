using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class ProductUI
    {
        public static void ShowProducts(List<Product> products)
        {
            foreach (var p in products)
                Console.WriteLine($"{p.PLU} - {p.Name} - {p.Price} kr");
        }

        public static string AskForPlu()
        {
            Console.Write("Ange PLU: ");
            return Console.ReadLine()!;
        }

        public static string AskForName()
        {
            Console.Write("Namn: ");
            return Console.ReadLine()!;
        }

        public static decimal AskForPrice()
        {
            Console.Write("Pris: ");
            decimal.TryParse(Console.ReadLine(), out decimal price);
            return price;
        }

        public static PriceUnit AskForUnit()
        {
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val: ");
            return Console.ReadLine() == "1" ? PriceUnit.PerKilo : PriceUnit.Each;
        }
    }
}
