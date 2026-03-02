using CashRegisterSystem.Models;
using System.Collections.Generic;

namespace CashRegisterSystem.Data.Interfaces
{
    /// <summary>
    /// Defines a writer that can output a receipt to any medium
    /// such as file, console or PDF.
    /// </summary>
    public interface IReceiptWriter
    {
        void WriteReceipt(
            List<CartItem> items,
            string cashierName,
            int registerNumber,
            string paymentMethod,
            decimal amountReceived);
    }
}
