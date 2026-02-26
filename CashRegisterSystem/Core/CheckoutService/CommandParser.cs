using System;

namespace CashRegisterSystem.Core.CheckoutService
{
    internal class CommandParser
    {
        public bool IsPayCommand(string input)
        {
            return input.Equals("PAY", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsCancelCommand(string input)
        {
            return input.Equals("CANCEL", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsDeleteCommand(string input)
        {
            return input.StartsWith("DEL", StringComparison.OrdinalIgnoreCase) ||
                   input.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase);
        }

        public string? ExtractPluFromDelete(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                return null;

            return parts[1];
        }
    }
}
