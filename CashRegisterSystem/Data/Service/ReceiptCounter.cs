namespace CashRegisterSystem.Data.Service
{
    public class ReceiptCounter
    {
        private static readonly string ReceiptFolderPath =
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Receipts");

        private static readonly string ReceiptNumberFilePath =
            Path.Combine(ReceiptFolderPath, "receipt_number.txt");

        public int GetNextReceiptNumber()
        {
            Directory.CreateDirectory(ReceiptFolderPath);

            int nextReceiptNumber = 1;

            if (File.Exists(ReceiptNumberFilePath))
            {
                string? content = File.ReadAllText(ReceiptNumberFilePath);

                if (int.TryParse(content, out int lastNumber) && lastNumber > 0)
                {
                    nextReceiptNumber = lastNumber + 1;
                }
            }

            File.WriteAllText(ReceiptNumberFilePath, nextReceiptNumber.ToString());

            return nextReceiptNumber;
        }
    }
}
