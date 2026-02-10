using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public class ReceiptService : IReceiptWriter
    {
        public void WriteReceipt(List<(ProductModel Product, decimal Quantity)> cart)
        {
            // Här skriver du kvittot till fil
        }
    }
}
