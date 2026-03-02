using System;
using System.Collections.Generic;
using System.Linq;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.CheckoutService
{
    /// <summary>
    /// Parses user input into a product and quantity pair.
    /// Ensures safe parsing and prevents crashes from invalid input.
    /// </summary>
    public class InputParser
    {
        public (Product product, decimal quantity)? Parse(string input, List<Product> products)
        {
            if (string.IsNullOrWhiteSpace(input) || products == null)
                return null;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                return null;

            string plu = parts[0].Trim();
            string quantityText = parts[1].Replace(".", ",").Trim();

            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null)
                return null;

            if (!decimal.TryParse(quantityText, out decimal quantity))
                return null;

            if (quantity <= 0)
                return null;

            return (product, quantity);
        }
    }
}
