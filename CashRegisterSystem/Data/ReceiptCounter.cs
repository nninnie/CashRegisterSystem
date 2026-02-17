namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Handles generation of sequential receipt numbers and ensures that the 
    /// current receipt number is persisted between program runs.
    /// </summary>
    /// <remarks>
    /// Receipt numbers are stored in a text file inside the Receipts folder. 
    /// If the file does not exist, numbering starts at 1.
    /// </remarks>
    public class ReceiptCounter
    {
        private static readonly string ReceiptFolderPath =
            Path.Combine(AppContext.BaseDirectory, "Receipts");

        private static readonly string ReceiptNumberFilePath =
            Path.Combine(ReceiptFolderPath, "receipt_number.txt");

        /// <summary>
        /// Retrieves the next available receipt number and updates the stored value.
        /// </summary>
        /// <returns>The next sequential receipt number.</returns>
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
