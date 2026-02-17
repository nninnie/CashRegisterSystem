namespace CashRegisterSystem.Core
{
    /// <summary>
    /// Represents an item in a shopping cart, including the selected product, quantity, and pricing details.
    /// </summary>
    public class CartItem
    {
        public required  Product Product { get; set; }
        public decimal Quantity { get; set; }

        public decimal DiscountAmount { get; set; } // Rabatt i kr
        public decimal TotalPrice => Product.Price * Quantity; // Ordinarie totalpris (pris * mängd)
        public decimal FinalPrice => TotalPrice - DiscountAmount; // Slutpris efter rabatt
    }
}