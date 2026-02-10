using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public class ReceiptWriter : IReceiptWriter
    {
        public void WriteReceipt(List<(Product Product, decimal Quantity)> cart)
        {
            // Här skriver du kvittot till fil
        }
    }
}
