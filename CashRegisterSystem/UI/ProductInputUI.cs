using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Handles all user input related to products.
    /// Provides simple and safe input methods for PLU, names, prices and units.
    /// All console text is in Swedish, while code and comments are in English.
    /// </summary>
    public static class ProductInputUI
    {
        /// <summary>
        /// Returns the correct price depending on whether a campaign is active.
        /// </summary>
        public static decimal GetEffectivePrice(Product product)
        {
            return CampaignService.GetPrice(product);
        }

        /// <summary>
        /// Asks the user to enter a PLU code.
        /// </summary>
        public static string AskForPlu()
        {
            return InputService.ReadString("Ange PLU: ");
        }

        /// <summary>
        /// Asks the user to enter a product name.
        /// </summary>
        public static string AskForProductName()
        {
            return InputService.ReadString("Produktnamn: ");
        }

        /// <summary>
        /// Asks the user to enter the base price.
        /// NOTE: Price is entered as a whole number (no decimals).
        /// </summary>
        public static decimal AskForBasePrice()
        {
            return InputService.ReadDecimal("Grundpris: ");
        }

        /// <summary>
        /// Lets the user choose whether the product is sold per kilo or per piece.
        /// </summary>
        public static PriceUnit AskForPriceUnit()
        {
            Console.WriteLine("1. Per kilo (kg)");
            Console.WriteLine("2. Per styck (st)");

            int choice = InputService.ReadInt("Välj enhet: ");

            return choice == 1 ? PriceUnit.PerKilo : PriceUnit.Each;
        }

        /// <summary>
        /// Shows a confirmation message in green text.
        /// </summary>
        public static void ShowConfirmation(string title, string details)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{title}");
            Console.ResetColor();

            Console.WriteLine(details);
            Console.WriteLine("\nÅtergå till meny...");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows an error message in red text.
        /// </summary>
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            Console.WriteLine("\nTryck valfri tangent för att fortsätta...");
            Console.ReadKey();
        }

        /// <summary>
        /// Asks the user to enter a campaign label if needed.
        /// </summary>
        public static string AskForCampaignLabel()
        {
            return InputService.ReadString("Kampanjnamn (t.ex. 'Äpplen 3 för 20'): ");
        }
    }
}
