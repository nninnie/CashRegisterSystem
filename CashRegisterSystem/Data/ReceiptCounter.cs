namespace CashRegisterSystem.Data
{
    public class ReceiptCounter
    {
        // Mapp där alla kvittofiler ska ligga
        private static readonly string ReceiptFolder =
            Path.Combine(AppContext.BaseDirectory, "Receipts");

        // Filen som sparar senaste kvittonumret
        private static readonly string ReceiptNumberFilePath =
            Path.Combine(ReceiptFolder, "receipt_number.txt");

        public int GetNextReceiptNumber()
        {
            // Skapa mappen om den inte finns
            Directory.CreateDirectory(ReceiptFolder);

            int nextReceiptNumber = 1; // Starta på 1 om filen inte finns

            // Om filen finns → läs senaste kvittonumret
            if (File.Exists(ReceiptNumberFilePath))
            {
                using (var reader = new StreamReader(ReceiptNumberFilePath))
                {
                    string receiptNumberInFile = reader.ReadToEnd();

                    if (int.TryParse(receiptNumberInFile, out int lastNumber))
                    {
                        nextReceiptNumber = lastNumber + 1;
                    }
                }
            }

            // Skriv det nya kvittonumret tillbaka till filen
            using (var writer = new StreamWriter(ReceiptNumberFilePath, false))
            {
                writer.Write(nextReceiptNumber);
            }

            return nextReceiptNumber;
        }
    }
}
