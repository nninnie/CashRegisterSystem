namespace CashRegisterSystem.Core
{
    public class Cart
    {
        public List<CartItem> CartItems { get; } = new();

        public void AddItem(Product product, decimal quantity)
        {
            CartItems.Add(new CartItem
            {
                Product = product,
                Quantity = quantity
            });
        }

        public decimal TotalPrice => CartItems.Sum(i => i.TotalPrice);

        public void Clear() => CartItems.Clear();
    }
}
