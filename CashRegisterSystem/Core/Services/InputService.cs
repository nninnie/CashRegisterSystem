using CashRegisterSystem.Models;
using System;
using System.Text;

namespace CashRegisterSystem.Core.Services
{
    /// <summary>
    /// Provides safe and validated console input for strings, numbers, dates and units.
    /// Supports ESC to cancel and return to previous menu.
    /// </summary>
    public static class InputService
    {
        /// <summary>
        /// Reads a line from the console and supports ESC to cancel.
        /// Returns null if ESC is pressed before Enter.
        /// </summary>
        private static string? ReadLineWithEsc()
        {
            var buffer = new StringBuilder();

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    // User pressed ESC → cancel input
                    Console.WriteLine(); // move to next line for clean UI
                    return null;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return buffer.ToString();
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 0)
                    {
                        buffer.Remove(buffer.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    buffer.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }
        }

        /// <summary>
        /// Reads a non-empty string. Returns null if ESC is pressed.
        /// </summary>
        public static string? ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = ReadLineWithEsc();
                if (input == null)
                    return null; // ESC

                input = input.Trim();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                ShowError("Fältet får inte vara tomt.");
            }
        }

        /// <summary>
        /// Reads a string that may be empty. Returns null if ESC is pressed.
        /// </summary>
        public static string? ReadStringAllowEmpty(string prompt)
        {
            Console.Write(prompt);

            string? input = ReadLineWithEsc();
            if (input == null)
                return null; // ESC

            return input;
        }

        /// <summary>
        /// Reads a valid integer. Returns null if ESC is pressed.
        /// </summary>
        public static int? ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = ReadLineWithEsc();
                if (input == null)
                    return null; // ESC

                if (int.TryParse(input, out int value))
                    return value;

                ShowError("Ogiltigt tal. Endast heltal är tillåtna.");
            }
        }

        /// <summary>
        /// Reads a valid decimal. Returns null if ESC is pressed.
        /// </summary>
        public static decimal? ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = ReadLineWithEsc();
                if (input == null)
                    return null; // ESC

                if (decimal.TryParse(input, out decimal value))
                    return value;

                ShowError("Ogiltigt belopp.");
            }
        }

        /// <summary>
        /// Reads a valid date. Returns null if ESC is pressed.
        /// </summary>
        public static DateTime? ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = ReadLineWithEsc();
                if (input == null)
                    return null; // ESC

                if (DateTime.TryParse(input, out DateTime date))
                    return date;

                ShowError("Ogiltigt datum. Ange i formatet ÅÅÅÅ-MM-DD.");
            }
        }

        /// <summary>
        /// Reads a valid price unit (kg or st). Returns null if ESC is pressed.
        /// </summary>
        public static PriceUnit? ReadUnit(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                string? input = ReadLineWithEsc();
                if (input == null)
                    return null; // ESC

                input = input.Trim().ToLower();

                if (input == "kg")
                    return PriceUnit.PerKilo;

                if (input == "st")
                    return PriceUnit.Each;

                ShowError("Ogiltig enhet. Skriv 'kg' eller 'st'.");
            }
        }

        /// <summary>
        /// Shows a red error message with consistent formatting.
        /// </summary>
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{message}\n");
            Console.ResetColor();
        }
    }
}
