using CashRegisterSystem.Models;

namespace CashRegisterSystem.Data.Interfaces
{
    /// <summary>
    /// Ansvarar för att skriva ett kvitto till valfritt medium (fil, konsol, etc.)
    /// </summary>
    public interface IReceiptWriter
    {
        void WriteReceipt(List<CartItem> items, string cashierName, int registerNumber, string paymentMethod, decimal amountReceived);
    }
}

