using CashRegisterSystem.Core;

namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Writes receipts to a text file and displays them in the console.
    /// </summary>
    /// <remarks>
    /// Each receipt is appended to a daily receipt file inside the Receipts folder.
    /// Receipt numbers are generated through <see cref="ReceiptCounter"/>.
    /// </remarks>
    public class ReceiptFileWriter : IReceiptWriter
    {
        private readonly ReceiptCounter _counter = new ReceiptCounter();

        /// <summary>
        /// Generates a formatted receipt for the provided cart items, saves it to a daily receipt file,
        /// and displays the receipt text in the console.
        /// </summary>
        /// <param name="cartItems">The items included in the customer's purchase.</param>
        public void WriteReceipt(List<CartItem> cartItems)
        {
            int receiptNumber = _counter.GetNextReceiptNumber();

            string receiptText = ReceiptUI.FormatReceipt(receiptNumber, cartItems);

            string receiptFolder = Path.Combine(AppContext.BaseDirectory, "Receipts");
            Directory.CreateDirectory(receiptFolder);

            string filePath = Path.Combine(receiptFolder, $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            File.AppendAllText(filePath, receiptText + Environment.NewLine);

            Console.Clear();
            Console.WriteLine(receiptText);
        }
    }
}
