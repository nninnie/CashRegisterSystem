using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Data.Interfaces
{
    public class ReceiptFileWriter : IReceiptWriter
    {
        private readonly ReceiptCounter _counter = new ReceiptCounter();

        public void WriteReceipt(List<CartItem> cartItems, string cashierName, int registerNumber, string paymentMethod, decimal amountReceived)
        {
            int receiptNumber = _counter.GetNextReceiptNumber();

            string receiptText = ReceiptUI.FormatReceipt(
                receiptNumber,
                cartItems,
                cashierName,
                registerNumber,
                paymentMethod,
                amountReceived
            );

            string receiptFolder = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Receipts");
            Directory.CreateDirectory(receiptFolder);

            string filePath = Path.Combine(receiptFolder, $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            File.AppendAllText(filePath, receiptText + Environment.NewLine);

            Console.Clear();
            Console.WriteLine(receiptText);
        }
    }
}