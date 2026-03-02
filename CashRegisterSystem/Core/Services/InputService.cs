using CashRegisterSystem.Models;
using System;

namespace CashRegisterSystem.Core.Services
{
    /// <summary>
    /// Provides safe input methods for strings, numbers and units.
    /// Ensures trimmed values and prevents crashes from invalid input.
    /// </summary>
    public static class InputService
    {
        /// <summary>
        /// Reads a non-empty string. Used in admin menus and product input.
        /// </summary>
        public static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fältet får inte vara tomt. Försök igen.\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Reads a string that may be empty. Used in login.
        /// </summary>
        public static string ReadStringAllowEmpty(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        /// <summary>
        /// Reads a valid integer. Used for prices (heltal) and quantities.
        /// </summary>
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int value))
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt tal. Endast heltal är tillåtna.\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Reads a valid decimal. Used for campaign prices and multi-buy prices.
        /// </summary>
        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out decimal value))
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt belopp. Försök igen.\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Reads a valid date in Swedish format. Used in campaign scheduling.
        /// </summary>
        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (DateTime.TryParse(input, out DateTime date))
                    return date;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt datum. Ange i formatet ÅÅÅÅ-MM-DD.\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Reads a valid price unit (kg or st). Prevents crashes and invalid input.
        /// </summary>
        public static PriceUnit ReadUnit(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (input == "kg")
                    return PriceUnit.PerKilo;

                if (input == "st")
                    return PriceUnit.Each;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltig enhet. Skriv 'kg' eller 'st'.\n");
                Console.ResetColor();
            }
        }
    }
}
