using System;
using System.Collections.Generic;
using System.Linq;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.CheckoutService
{
    public class InputParser
    {
        public (Product product, decimal quantity)? Parse(string input, List<Product> products)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                Console.WriteLine("Fel format. Skriv in PLU (mellanslag) antal/kg");
                return null;
            }

            string plu = parts[0];
            string quantityText = parts[1].Replace(".", ",");

            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product is null)
            {
                Console.WriteLine("Produkt hittades inte. Försök igen.");
                return null;
            }

            if (!decimal.TryParse(quantityText, out decimal quantity) || quantity <= 0)
            {
                Console.WriteLine("Ogiltigt antal/vikt.");
                return null;
            }

            return (product, quantity);
        }
    }
}
