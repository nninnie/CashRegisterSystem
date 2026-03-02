using System;

namespace CashRegisterSystem.Core.Services
{
    /// <summary>
    /// Provides safe input methods for strings, numbers and dates.
    /// Ensures trimmed values and prevents crashes from invalid input.
    /// </summary>
    public static class InputService
    {
        // Strict string input (used in menus, admin, product input).
        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            return input.Trim();
        }

        // Allows empty string (used in login).
        public static string ReadStringAllowEmpty(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        // Reads an integer safely.
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int value))
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt tal. Försök igen.");
                Console.ResetColor();
            }
        }

        // Reads a decimal safely.
        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out decimal value))
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt belopp. Försök igen.");
                Console.ResetColor();
            }
        }

        // Reads a date safely (used in campaigns).
        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (DateTime.TryParse(input, out DateTime date))
                    return date;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt datum. Ange i formatet ÅÅÅÅ-MM-DD.");
                Console.ResetColor();
            }
        }
    }
}
