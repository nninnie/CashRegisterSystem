using CashRegisterSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Ansvarar för att skriva ett kvitto till valfritt medium (fil, konsol, etc.)
    /// </summary>
    public interface IReceiptWriter
    {
        /// <summary>
        /// Skriver ett kvitto baserat på kundens varukorg.
        /// </summary>
        /// <param name="cartItems"></param>
        void WriteReceipt(List<CartItem> cartItems);
    }
}

