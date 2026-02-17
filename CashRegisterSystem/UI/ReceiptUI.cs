using CashRegisterSystem.Core;
using System.Text;

namespace CashRegisterSystem.Data
{
    public static class ReceiptUI
    {
        /// <summary>
        /// Formats a detailed receipt as a string for the specified receipt number and list of purchased items.
        /// </summary>
        /// <remarks>The returned receipt string includes store details, a timestamp, an itemized list of
        /// products with quantities and prices, VAT calculations, and a payment summary. The method assumes all items
        /// are subject to a 12% VAT rate. The formatting is suitable for display or printing, but does not include any
        /// graphical elements.</remarks>
        /// <param name="receiptNumber">The unique number identifying the receipt. Used to label the generated receipt output.</param>
        /// <param name="items">The list of cart items to include on the receipt. Each item represents a purchased product and its quantity.
        /// Cannot be null.</param>
        /// <returns>A formatted string representing the complete receipt, including store information, itemized purchases,
        /// totals, VAT breakdown, and payment summary.</returns>
        public static string FormatReceipt(int receiptNumber, List<CartItem> items)
        {
            var sb = new StringBuilder();

            // --- Butiksinfo ---
            sb.AppendLine("========================================");
            sb.AppendLine("                 KVITTO");
            sb.AppendLine("          Ammo's Grönsakslivs");
            sb.AppendLine("          Öppet 08:00 - 23:00");
            sb.AppendLine("          Tel: 073-570 43 68");
            sb.AppendLine("========================================");

            sb.AppendLine($"Kvitto #{receiptNumber}");
            sb.AppendLine($"Datum: {DateTime.Now:yyyy-MM-dd HH:mm}");
            sb.AppendLine();

            // --- Varor ---
            decimal total = 0;

            foreach (var item in items)
            {
                string unit = item.Product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                decimal lineTotal = item.TotalPrice;
                total += lineTotal;

                // Namn (vänster), mängd + enhet + pris (mitten), total (höger)
                sb.AppendLine($"{item.Product.Name,-13} {item.Quantity} {unit} á {item.Product.Price,6:F2}   {lineTotal,7:F2} kr");
            }

            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"SUMMA VAROR: {total,24:F2} kr");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();


            // --- Moms ---
            decimal vatRate = 0.12m;
            decimal vatAmount = total * vatRate / (1 + vatRate);
            decimal exVat = total - vatAmount;



            sb.AppendLine("MOMSREDOVISNING");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"12% moms:{vatAmount,28:F2} kr");
            sb.AppendLine($"Exkl. moms:{exVat,26:F2} kr");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"ATT BETALA:{total,26:F2} kr");
            sb.AppendLine("========================================");

            // --- Avslut ---
            sb.AppendLine("           Tack för ditt köp!");
            sb.AppendLine("             Välkommen åter!");
            sb.AppendLine("           Org.nr: 987654-3210  ");
            sb.AppendLine("========================================");

            return sb.ToString();
        }
    }
}
