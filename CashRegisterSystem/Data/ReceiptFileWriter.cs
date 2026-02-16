using CashRegisterSystem.Core;

namespace CashRegisterSystem.Data
{
    public class ReceiptFileWriter : IReceiptWriter
    {
        private readonly ReceiptCounter _counter = new ReceiptCounter();

        public void WriteReceipt(List<CartItem> cartItems)
        {
            int receiptNumber = _counter.GetNextReceiptNumber();

            // Skapa kvittot som text via FormatReceipt
            string receiptText = ReceiptUI.FormatReceipt(receiptNumber, cartItems);

            // Mapp för kvitton
            string folder = Path.Combine(AppContext.BaseDirectory, "Receipts");
            Directory.CreateDirectory(folder);

            // Filnamn baserat på dagens datum
            string fileName = Path.Combine(folder, $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            // === 1. Skriv kvittot till fil ===
            using (var writer = new StreamWriter(fileName, append: true))
            {
                writer.WriteLine(receiptText);
            }

            // === 2. Visa kvittot i konsolen ===
            Console.Clear();
            Console.WriteLine(receiptText);

            Console.WriteLine("\nKöp avslutat. Tryck ENTER för ny kund.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
