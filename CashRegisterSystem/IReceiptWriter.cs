using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public interface IReceiptWriter
    {
        void WriteReceipt(List<(ProductModel Product, decimal Quantity)> cart);
    }
}

