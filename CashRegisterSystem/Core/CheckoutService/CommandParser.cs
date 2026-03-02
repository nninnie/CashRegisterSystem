using System;

namespace CashRegisterSystem.Core.CheckoutService
{
    /// <summary>
    /// Parses checkout commands such as PAY, CANCEL and DEL <PLU>.
    /// Ensures safe string handling and prevents crashes from invalid input.
    /// </summary>
    internal class CommandParser
    {
        public bool IsPayCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return input.Equals("PAY", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsCancelCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return input.Equals("CANCEL", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsDeleteCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return input.StartsWith("DEL", StringComparison.OrdinalIgnoreCase) ||
                   input.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase);
        }

        public string? ExtractPluFromDelete(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                return null;

            string plu = parts[1].Trim();

            return string.IsNullOrWhiteSpace(plu) ? null : plu;
        }
    }
}
