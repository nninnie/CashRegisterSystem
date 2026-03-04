using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CashRegisterSystem.Data.Interfaces
{
    /// <summary>
    /// Writes receipts to text files and prints them to the console.
    /// Automatically generates the next receipt number.
    /// </summary>
    public class ReceiptFileWriter : IReceiptWriter
    {
        // Returns the next available receipt number based on existing files.
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

        // Writes the receipt to a file and prints it to the console.
        public void WriteReceipt(
            List<CartItem> cartItems,
            string cashierName,
            int registerNumber,
            string paymentMethod,
            decimal amountReceived)
        {
            if (cartItems == null || cartItems.Count == 0)
                return;

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
                $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");


            File.WriteAllText(filePath, receiptText);

            Console.Clear();
            Console.WriteLine(receiptText);
        }
    }
}
