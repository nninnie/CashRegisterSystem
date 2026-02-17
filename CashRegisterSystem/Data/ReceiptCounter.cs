namespace CashRegisterSystem.Data
{
    public class ReceiptCounter
    {
        // Mapp där alla kvittofiler ska ligga
        private static readonly string ReceiptFolderPath =
            Path.Combine(AppContext.BaseDirectory, "Receipts");

        // Filen som sparar senaste kvittonumret
        private static readonly string ReceiptNumberFilePath =
            Path.Combine(ReceiptFolderPath, "receipt_number.txt");

        public int GetNextReceiptNumber()
        {
            // Skapa mappen om den inte finns
            Directory.CreateDirectory(ReceiptFolderPath);

            int nextReceiptNumber = 1; // Starta på 1 om filen inte finns

            // Om filen finns → läs senaste kvittonumret
            if (File.Exists(ReceiptNumberFilePath))
            {
                using (var readReceiptNumberFromFile = new StreamReader(ReceiptNumberFilePath))
                {
                    string receiptNumberFromFile = readReceiptNumberFromFile.ReadToEnd();

                    if (int.TryParse(receiptNumberFromFile, out int lastReceiptNumber))
                    {
                        nextReceiptNumber = lastReceiptNumber + 1;
                    }
                }
            }

            // Skriv det nya kvittonumret tillbaka till filen
            using (var writeReceiptNumberToFile = new StreamWriter(ReceiptNumberFilePath, false))
            {
                writeReceiptNumberToFile.Write(nextReceiptNumber);
            }

            return nextReceiptNumber;
        }
    }
}
