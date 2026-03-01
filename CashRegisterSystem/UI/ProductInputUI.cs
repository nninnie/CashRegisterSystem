using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class ProductInputUI
    {
        public static decimal GetPrice(Product product)
        {
            return CampaignService.GetPrice(product);
        }

        public static string AskForPlu()
        {
            return InputService.ReadString("Ange PLU: ");
        }

        public static string AskForName()
        {
            return InputService.ReadString("Namn: ");
        }

        public static decimal AskForPrice()
        {
            return InputService.ReadDecimal("Pris: ");
        }

        public static PriceUnit AskForUnit()
        {
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");

            int choice = InputService.ReadInt("Val: ");

            return choice == 1 ? PriceUnit.PerKilo : PriceUnit.Each;
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
