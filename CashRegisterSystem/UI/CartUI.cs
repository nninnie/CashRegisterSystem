using System;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays the shopping cart in a clean, structured checkout format.
    /// Uses fixed-width columns to mimic a real cash register layout.
    /// </summary>
    public static class CartUI
    {
        private const int CartWidth = 70;
        private const int NameColumnWidth = 30;     
        private const int QuantityColumnWidth = 10; 
        private const int PriceColumnWidth = 10;    

        /// <summary>
        /// Prints all items currently in the cart with aligned columns.
        /// </summary>
        public static void ShowCart(Cart cart)
        {
            Console.WriteLine("------------------------------KUNDVAGN--------------------------------\n");

            decimal totalSum = 0;

            foreach (var item in cart.CartItems)
            {
                Console.WriteLine(BuildCartLine(item));
                totalSum += item.TotalPrice - item.DiscountAmount;
            }

            Console.WriteLine("----------------------------------------------------------------------");

            string totalText = $"Total: {totalSum:0.00} kr";
            Console.WriteLine(totalText.PadLeft(CartWidth));
        }

        /// <summary>
        /// Builds a single cart line with left-aligned name, centered quantity,
        /// and fully right-aligned price. Ensures nothing goes outside the cart width.
        /// </summary>
        private static string BuildCartLine(CartItem item)
        {
            string name = item.Product.Name ?? "(saknas)";
            if (name.Length > NameColumnWidth)
                name = name.Substring(0, NameColumnWidth - 3) + "...";

            string nameCol = name.PadRight(NameColumnWidth);

            string qty = item.Product.PriceUnit == PriceUnit.PerKilo
                ? $"{item.Weight:F2} kg"
                : $"{item.Quantity} st";

            int qtyPadding = (QuantityColumnWidth - qty.Length) / 2;
            if (qtyPadding < 0) qtyPadding = 0;

            string qtyCol =
                new string(' ', qtyPadding) +
                qty +
                new string(' ', QuantityColumnWidth - qtyPadding - qty.Length);

            decimal total = item.TotalPrice - item.DiscountAmount;
            string priceText = $"{total:0.00} kr";

            int usedWidth = NameColumnWidth + QuantityColumnWidth;
            int spacesBeforePrice = CartWidth - usedWidth - priceText.Length;
            if (spacesBeforePrice < 1) spacesBeforePrice = 1;

            string priceCol = new string(' ', spacesBeforePrice) + priceText;

            return $"{nameCol}{qtyCol}{priceCol}";
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
