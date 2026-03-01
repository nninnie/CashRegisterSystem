using CashRegisterSystem.Models;
using System.Text;

namespace CashRegisterSystem.Data
{
    public static class ReceiptUI
    {
        public static string FormatReceipt(
            int receiptNumber,
            List<CartItem> items,
            string cashierName,
            int registerNumber,
            string paymentMethod,
            decimal amountReceived = 0)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var sb = new StringBuilder();

            // --- Header ---
            sb.AppendLine("========================================");
            sb.AppendLine("                FruktTorget");
            sb.AppendLine("             Öppet 08:00 - 23:00");
            sb.AppendLine("             Tel: 073-570 43 68");
            sb.AppendLine("========================================");

            sb.AppendLine($"Säljare: {cashierName}".PadRight(19) + $"Kassa: {registerNumber}".PadRight(11) + $"Kvitto: {receiptNumber}");
            sb.AppendLine($"Datum: {DateTime.Now:yyyy-MM-dd}".PadRight(30) + $"Tid: {DateTime.Now:HH:mm}");
            sb.AppendLine("----------------------------------------");

            // --- Items ---
            decimal total = 0;

            foreach (var item in items)
            {
                if (item?.Product == null)
                    continue;

                string unit = item.Product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                decimal lineTotal = item.TotalPrice;
                total += lineTotal;

                sb.AppendLine($"{item.Product.Name,-12}    {item.Quantity} *{item.Product.Price,6:F2}  {lineTotal,10:F2} kr");
            }

            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"TOTALT:{total,30:F2} kr");
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

            // --- Payment ---
            sb.AppendLine();

            if (paymentMethod == "Kontant")
            {
                decimal change = amountReceived - total;
                sb.AppendLine($"Mottaget kontant:{amountReceived,18:F2} kr");
                sb.AppendLine($"Åter kontant:{change,22:F2} kr");
            }
            else if (paymentMethod == "Kort")
            {
                sb.AppendLine("Betalt med kort");
            }

            sb.AppendLine("========================================");

            // --- Footer ---
            sb.AppendLine("     Tack för besöket, spara kvittot");
            sb.AppendLine("            Välkommen åter!");
            sb.AppendLine("          Org.nr: 987654-3210");
            sb.AppendLine("========================================");

            return sb.ToString();
        }
    }
}
