namespace CashRegisterSystem.Models
{
    /// <summary>
    /// Represents an item in the shopping cart.
    /// Stores product, quantity/weight, unit price and discount.
    /// Calculates total and final price safely.
    /// </summary>
    public class CartItem
    {
        public required Product Product { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => _quantity = value < 0 ? 0 : value; // Prevents negative values
        }

        private decimal _weight;
        public decimal Weight
        {
            get => _weight;
            set => _weight = value < 0 ? 0 : value; // Prevents negative values
        }

        public decimal UnitPrice { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalPrice =>
            Product.PriceUnit == PriceUnit.PerKilo
                ? UnitPrice * Weight
                : UnitPrice * Quantity;

        public decimal FinalPrice => TotalPrice - DiscountAmount;
    }
}
