using CashRegisterSystem.Core;
using System.Text;

namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Provides formatting utilities for generating a complete receipt as a text string.
    /// </summary>
    public static class ReceiptUI
    {
        /// <summary>
        /// Formats a detailed receipt string for the specified receipt number and purchased items.
        /// </summary>
        /// <remarks>
        /// The receipt includes store information, timestamp, itemized purchases, totals,
        /// VAT breakdown (12%), and a closing message. The result is suitable for printing
        /// or console display.
        /// </remarks>
        /// <param name="receiptNumber">The unique receipt number.</param>
        /// <param name="items">The purchased items to include on the receipt.</param>
        /// <returns>A fully formatted receipt as a string.</returns>
        public static string FormatReceipt(int receiptNumber, List<CartItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var sb = new StringBuilder();

            // --- Header ---
            sb.AppendLine("========================================");
            sb.AppendLine("                 KVITTO");
            sb.AppendLine("              FruktTorget");
            sb.AppendLine("          Öppet 08:00 - 23:00");
            sb.AppendLine("          Tel: 073-570 43 68");
            sb.AppendLine("========================================");

            sb.AppendLine($"Kvitto #{receiptNumber}");
            sb.AppendLine($"Datum: {DateTime.Now:yyyy-MM-dd HH:mm}");
            sb.AppendLine();
            sb.AppendLine();

            // --- Items ---
            decimal total = 0;

            foreach (var item in items)
            {
                if (item?.Product == null)
                    continue; // skyddar mot trasiga objekt

                string unit = item.Product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                decimal lineTotal = item.TotalPrice;
                total += lineTotal;

                sb.AppendLine($"{item.Product.Name,-13} {item.Quantity} {unit} á {item.Product.Price,6:F2}   {lineTotal,7:F2} kr");
            }

            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"SUMMA VAROR: {total,24:F2} kr");
            sb.AppendLine();
            sb.AppendLine();

            // --- VAT ---
            const decimal vatRate = 0.12m;
            decimal vatAmount = total * vatRate / (1 + vatRate);
            decimal exVat = total - vatAmount;

            sb.AppendLine("MOMSREDOVISNING");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"12% moms:{vatAmount,28:F2} kr");
            sb.AppendLine($"Exkl. moms:{exVat,26:F2} kr");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"ATT BETALA:{total,26:F2} kr");
            sb.AppendLine("========================================");

            // --- Footer ---
            sb.AppendLine("           Tack för ditt köp!");
            sb.AppendLine("             Välkommen åter!");
            sb.AppendLine("           Org.nr: 987654-3210");
            sb.AppendLine("========================================");

            return sb.ToString();
        }
    }
}
