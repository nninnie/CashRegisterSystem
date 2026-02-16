using CashRegisterSystem.Core;
using System;
using System.Linq;
using System.Text;

namespace CashRegisterSystem.Data
{
    public static class ReceiptUI
    {
        // Bygger och returnerar hela kvittot som en färdig textsträng
        public static string FormatReceipt(int receiptNumber, List<CartItem> items)
        {
            // Effektivt sätt att bygga text rad för rad
            var sb = new StringBuilder();

            // Totalpris inkl. moms
            decimal total = items.Sum(i => i.TotalPrice);

            // Momsberäkning (12% för livsmedel) 
            // Priserna är inkl. moms → därför räknar vi ut hur mycket av totalen som är moms
            decimal vatRate = 0.12m;
            decimal vatAmount = total * vatRate / (1 + vatRate); // Formeln för att plocka ut moms ur ett pris inkl. moms
            decimal exVat = total - vatAmount; // Pris exklusive moms

            sb.AppendLine("========================================");
            sb.AppendLine("                 KVITTO");
            sb.AppendLine("========================================");
            sb.AppendLine($"Kvitto #{receiptNumber}");
            sb.AppendLine($"Datum: {DateTime.Now:yyyy-MM-dd HH:mm}");
            sb.AppendLine();

            // Varje radvara skrivs ut med namn, mängd, enhet, pris och radtotal
            foreach (var item in items)
            {
                string unit = item.Product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                sb.AppendLine($"{item.Product.Name,-15} {item.Quantity} {unit} á {item.Product.Price}   {item.TotalPrice:F2} kr");
            }

            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"SUMMA VAROR: {total:F2} kr");
            sb.AppendLine();
            sb.AppendLine("MOMSREDOVISNING");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"12% moms:      {vatAmount:F2} kr");
            sb.AppendLine($"Exkl. moms:    {exVat:F2} kr");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"ATT BETALA:    {total:F2} kr");
            sb.AppendLine("========================================");
            sb.AppendLine("Tack för ditt köp!");
            sb.AppendLine("Välkommen åter!");
            sb.AppendLine("========================================");


            // Returnerar hela kvittot som en textsträng
            return sb.ToString();
        }
    }
}
