using System;
using System.Collections.Generic;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays the shopping cart in a clean, structured checkout format.
    /// Uses fixed-width columns to mimic a real cash register layout.
    /// </summary>
    public static class CartUI
    {
        // Total width of the cart display area
        private const int Width = 70;

        // Column widths for a real checkout layout
        private const int NameWidth = 25;   // Product name
        private const int QtyWidth = 6;     // Quantity or weight
        private const int UnitWidth = 3;    // st / kg
        private const int TotalWidth = 10;  // Total amount

        /// <summary>
        /// Prints all items currently in the cart with aligned columns.
        /// </summary>
        public static void ShowCart(Cart cart)
        {
            Console.WriteLine("------------------------------KUNDVAGN--------------------------------\n");

            decimal totalSum = 0;

            foreach (var item in cart.CartItems)
            {
                // Null-safety for product name
                string name = item.Product.Name ?? "(saknas namn)";

                // Truncate long names to fit the column
                if (name.Length > NameWidth)
                    name = name[..(NameWidth - 3)] + "...";

                name = name.PadRight(NameWidth);

                string qtyText;
                string unitLabel;

                // Weight-based product
                if (item.Product.PriceUnit == PriceUnit.PerKilo)
                {
                    qtyText = $"{item.Weight:F2}".PadLeft(QtyWidth);
                    unitLabel = "kg";
                }
                else // Piece-based product
                {
                    qtyText = $"{item.Quantity}".PadLeft(QtyWidth);
                    unitLabel = "st";
                }

                // Calculate total price including discount
                decimal total = item.TotalPrice - item.DiscountAmount;
                totalSum += total;

                string totalColumn = $"{total:0.00} kr".PadLeft(TotalWidth);

                // Real checkout style:
                // [Name][Qty][Unit][Total]
                Console.WriteLine($"{name}{qtyText} {unitLabel.PadRight(UnitWidth)}{totalColumn}");
            }

            Console.WriteLine("----------------------------------------------------------------------");

            // TOTAL line – right aligned
            string totalText = $"Total: {totalSum:0.00} kr";
            Console.WriteLine(totalText.PadLeft(Width));
        }

        /// <summary>
        /// Shows the PLU input prompt used during checkout.
        /// </summary>
        public static void ShowPluInputPrompt()
        {
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine(" PLU [mellanslag] antal/vikt(kg)    |    (PAY / CANCEL / DELETE PLU) ");
            Console.WriteLine("----------------------------------------------------------------------\n");
        }
    }
}
