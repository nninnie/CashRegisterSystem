namespace CashRegisterSystem.Core
{
    public class Cart
    {
        // Listan som håller alla varor kunden lagt i varukorgen.
        // Varje rad är ett CartItem (produkt + mängd + radpris).
        public List<CartItem> CartItems { get; } = new();

        // Lägger till en ny vara i varukorgen.
        // Skapar ett CartItem-objekt och stoppar in det i listan.
        public void AddItem(Product product, decimal quantity)
        {
            CartItems.Add(new CartItem
            {
                Product = product,
                Quantity = quantity
            });
        }

        // Räknar ut totalpriset för hela varukorgen.
        // Summerar alla radpriser (CartItem.TotalPrice).
        public decimal TotalPrice => CartItems.Sum(i => i.TotalPrice);

        // Tömmer varukorgen, t.ex. efter betalning.
        public void Clear() => CartItems.Clear();
    }
}
