namespace CashRegisterSystem.Models
{
    public class CartItem
    {
        public required Product Product { get; set; }
        public decimal Quantity { get; set; }

        // Pris per enhet vid köptillfället (ordinarie eller kampanjpris)
        public decimal UnitPrice { get; set; }

        // Rabatt i kronor (beräknad av DiscountService)
        public decimal DiscountAmount { get; set; }

        // Totalpris innan rabatt
        public decimal TotalPrice => UnitPrice * Quantity;

        // Slutpris efter rabatt
        public decimal FinalPrice => TotalPrice - DiscountAmount;
    }
}