using CashRegisterSystem.Models;

namespace CashRegisterSystem.Data.Interfaces
{
    public class ReceiptFileWriter : IReceiptWriter
    {
        private int GetNextReceiptNumber()
        {
            string receiptFolder = Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "Receipts");

            if (!Directory.Exists(receiptFolder))
                return 1;

            var files = Directory.GetFiles(receiptFolder, "RECEIPT_*.txt");

            if (files.Length == 0)
                return 1;

            int maxNumber = files
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .Select(name => name.Replace("RECEIPT_", ""))
                .Where(num => int.TryParse(num, out _))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max();

            return maxNumber + 1;
        }

        public void WriteReceipt(
            List<CartItem> cartItems,
            string cashierName,
            int registerNumber,
            string paymentMethod,
            decimal amountReceived)
        {
            int receiptNumber = GetNextReceiptNumber();

            string receiptText = ReceiptUI.FormatReceipt(
                receiptNumber,
                cartItems,
                cashierName,
                registerNumber,
                paymentMethod,
                amountReceived
            );

            string receiptFolder = Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "Receipts");

            Directory.CreateDirectory(receiptFolder);

            string filePath = Path.Combine(
                receiptFolder,
                $"RECEIPT_{receiptNumber}.txt");

            File.WriteAllText(filePath, receiptText);

            Console.Clear();
            Console.WriteLine(receiptText);
        }
    }
}
