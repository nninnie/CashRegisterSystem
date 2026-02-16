using CashRegisterSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CashRegisterSystem.Data
{
    public class ReceiptFileWriter : IReceiptWriter
    {
        private readonly ReceiptCounter _counter = new ReceiptCounter();
        public void WriteReceipt(List<(Product Product, decimal Quantity)> cart)
        {
            int receiptNumber = _counter.GetNextReceiptNumber();
            string fileName = $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt";

            using (var writer = new StreamWriter(fileName, append: true))
            {
                writer.WriteLine("========================================");
                writer.WriteLine($"Kvitto #{receiptNumber}");
                writer.WriteLine($"Datum: {DateTime.Now}");
                writer.WriteLine("----------------------------------------");


                decimal total = 0;

                foreach (var product in cart)
                {
                    decimal lineTotal = product.Product.Price * product.Quantity;
                    total += lineTotal;

                    writer.WriteLine($"{product.Product.Name} x{product.Quantity} á {product.Product.Price} kr = {lineTotal} kr");
                }
                writer.WriteLine("----------------------------------------");
                writer.WriteLine($"TOTALT: {total} kr");
                writer.WriteLine("========================================");
                writer.WriteLine();


            }
        }
    }
}
