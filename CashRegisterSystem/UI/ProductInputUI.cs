using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides all user input functions related to products.
    /// Each method asks for one specific value (PLU, name, price, unit, etc.)
    /// and safely returns null if the user presses ESC. 
    /// The class focuses only on collecting input and showing simple
    /// confirmation or error messages in the console.
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
        /// Returns null if ESC is pressed.
        /// </summary>
        public static string? AskForPlu()
        {
            return InputService.ReadString("\nPLU: ");
        }

        /// <summary>
        /// Asks the user to enter a product name.
        /// Returns null if ESC is pressed.
        /// </summary>
        public static string? AskForProductName()
        {
            return InputService.ReadString("Namn: ");
        }

        /// <summary>
        /// Asks the user to enter the base price as a decimal number.
        /// Returns null if ESC is pressed.
        /// </summary>
        public static decimal? AskForBasePrice()
        {
            return InputService.ReadDecimal("Pris: ");
        }

        /// <summary>
        /// Lets the user choose whether the product is sold per kilo or per piece.
        /// Returns null if ESC is pressed.
        /// </summary>
        public static PriceUnit? AskForPriceUnit()
        {
            Console.WriteLine("1. Per kilo (kg)");
            Console.WriteLine("2. Per styck (st)");

            int? choice = InputService.ReadInt("Välj enhet: ");
            if (choice == null)
                return null; 

            return choice.Value == 1 ? PriceUnit.PerKilo : PriceUnit.Each;
        }

        /// <summary>
        /// Shows a confirmation message in green text.
        /// </summary>
        public static void ShowConfirmation(string title, string details)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(title);
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine(details);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nTryck ENTER eller ESC för att fortsätta.");
            Console.ResetColor();

            Console.ReadKey(true);
            Console.Clear();
        }



        /// <summary>
        /// Shows an error message in red text.
        /// </summary>
        public static void ShowError(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nTryck ENTER eller ESC för att fortsätta.");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }


        /// <summary>
        /// Asks the user to enter a campaign label if needed.
        /// Returns null if ESC is pressed.
        /// </summary>
        public static string? AskForCampaignLabel()
        {
            return InputService.ReadString("Kampanjnamn (t.ex. 'Äpplen 3 för 20'): ");
        }
    }
}
