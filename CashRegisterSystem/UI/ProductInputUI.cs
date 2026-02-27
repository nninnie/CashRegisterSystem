using CashRegisterSystem.Models;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.UI
{
    public static class ProductInputUI
    {
      
        public static decimal GetPrice(Product product)
        {
            var campaigns = CampaignFileLoader.GetCampaigns();

            var activeCampaign = campaigns
                .FirstOrDefault(c => c.Name == product.Name && c.IsActive);

            if (activeCampaign == null)
                return product.Price;

            return product.Price * (1 - activeCampaign.Discount / 100);
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

        public static void ShowConfirmation(string title, string details)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{title}");
            Console.ResetColor();

            Console.WriteLine(details);

            Console.WriteLine("\nTryck valfri tangent för att fortsätta...");
            Console.ReadKey();
        }
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            Console.WriteLine("\nTryck valfri tangent för att fortsätta...");
            Console.ReadKey();
        }


    }
}