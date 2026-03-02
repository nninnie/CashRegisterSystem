namespace CashRegisterSystem.Models
{
    /// <summary>
    /// Represents an item in the shopping cart.
    /// Stores product, quantity/weight, unit price and discount.
    /// Calculates total and final price safely.
    /// </summary>
    public class CartItem
    {
        // The product connected to this cart item.
        public required Product Product { get; set; }

        // Quantity in pieces (used when price is Each).
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => _quantity = value < 0 ? 0 : value; // Prevents negative values
        }

        // Weight in kilograms (used when price is PerKilo).
        private decimal _weight;
        public decimal Weight
        {
            get => _weight;
            set => _weight = value < 0 ? 0 : value; // Prevents negative values
        }

        // The price per unit (piece or kilo).
        public decimal UnitPrice { get; set; }

        // Discount applied to this item.
        public decimal DiscountAmount { get; set; }

        // Calculates the total price before discount.
        public decimal TotalPrice =>
            Product.PriceUnit == PriceUnit.PerKilo
                ? UnitPrice * Weight
                : UnitPrice * Quantity;

        // Calculates the final price after discount.
        public decimal FinalPrice => TotalPrice - DiscountAmount;
    }
}
