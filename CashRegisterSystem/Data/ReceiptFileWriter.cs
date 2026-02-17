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
            string showTextStringReceipt = ReceiptUI.FormatReceipt(receiptNumber, cartItems);

            // Mapp för kvitton
            string createFolder = Path.Combine(AppContext.BaseDirectory, "Receipts");
            Directory.CreateDirectory(createFolder);

            // Filnamn baserat på dagens datum
            string fileNameInFolder = Path.Combine(createFolder, $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            // Skriv kvittot till fil
            // append: true: varje nytt kvitto läggs längst ner i samma fil så inte nya kvitton raderar den gamla
            using (var receiptWriterToFile = new StreamWriter(fileNameInFolder, append: true))
            {
                receiptWriterToFile.WriteLine(showTextStringReceipt);
            }

            // Visa kvittot i konsolen
            Console.Clear();
            Console.WriteLine(showTextStringReceipt);

            Console.WriteLine("\nKöp avslutat. Tryck ENTER för ny kund.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
