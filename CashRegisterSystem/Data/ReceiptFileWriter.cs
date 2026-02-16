using CashRegisterSystem.Core;

namespace CashRegisterSystem.Data
{
    public class ReceiptFileWriter : IReceiptWriter
    {
        // Ansvarar för att hålla koll på löpande kvittonummer
        private readonly ReceiptCounter _counter = new ReceiptCounter();

        public void WriteReceipt(List<CartItem> cartItems)
        {
            // Hämtar nästa kvittonummer (ökar automatiskt och sparas i fil)
            int receiptNumber = _counter.GetNextReceiptNumber();

            // Filnamn baserat på dagens datum (alla kvitton för dagen hamnar i samma fil)
            string fileName = $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt";

            // Öppnar filen och lägger till kvittot längst ner (append = true)
            using (var writer = new StreamWriter(fileName, append: true))
            {
                writer.WriteLine("========================================");
                writer.WriteLine($"Kvitto #{receiptNumber}");
                writer.WriteLine($"Datum: {DateTime.Now}");
                writer.WriteLine("----------------------------------------");

                decimal total = 0; // Håller koll på totalpriset

                // Skriver ut varje vara i kvittot
                foreach (var product in cartItems)
                {
                    decimal lineTotal = product.Product.Price * product.Quantity;
                    total += lineTotal; // Lägg till radens pris i totalen

                    writer.WriteLine($"{product.Product.Name} x{product.Quantity} á {product.Product.Price} kr = {lineTotal} kr");
                }

                writer.WriteLine("----------------------------------------");
                writer.WriteLine($"TOTALT: {total} kr"); // Totalpris för hela köpet
                writer.WriteLine("========================================");
                writer.WriteLine(); // Tom rad mellan kvitton
            }
        }
    }
}
