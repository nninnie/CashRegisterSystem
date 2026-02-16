using CashRegisterSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Data
{
    public interface IReceiptWriter
    {
        void WriteReceipt(List<CartItem> cartItems);
    }
}

