using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class ProductInputUI
    {
        // Returns the correct price based on active campaign
        public static decimal GetEffectivePrice(Product product)
        {
            return CampaignService.GetPrice(product);
        }

        // PLU input
        public static string AskForPlu()
        {
            return InputService.ReadString("Enter PLU: ");
        }

        // Product name input
        public static string AskForProductName()
        {
            return InputService.ReadString("Product name: ");
        }

        // Base price input
        public static decimal AskForBasePrice()
        {
            return InputService.ReadDecimal("Base price: ");
        }

        // Unit selection (kg or each)
        public static PriceUnit AskForPriceUnit()
        {
            Console.WriteLine("1. Per kilo (kg)");
            Console.WriteLine("2. Each (st)");

            int choice = InputService.ReadInt("Select unit: ");

            return choice == 1 ? PriceUnit.PerKilo : PriceUnit.Each;
        }

        // Confirmation message
        public static void ShowConfirmation(string title, string details)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{title}");
            Console.ResetColor();

            Console.WriteLine(details);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Error message
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Campaign name input (if needed)
        public static string AskForCampaignLabel()
        {
            return InputService.ReadString("Campaign label (e.g. 'Apples 3 for 20'): ");
        }
    }
}