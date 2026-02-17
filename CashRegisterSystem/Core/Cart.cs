namespace CashRegisterSystem.Core
{
    /// <summary>
    /// Represents a shopping cart that manages a collection of items selected for purchase, including their quantities
    /// and discounts.
    /// </summary>
    /// <remarks>The Cart class provides methods to add products with optional discounts, calculate the total
    /// price, and clear all items. Items are grouped by product; adding the same product increases its quantity and
    /// discount rather than creating a duplicate entry.</remarks>
    public class Cart
    {
        public List<CartItem> CartItems { get; } = new();

        /// <summary>
        /// Adds a product to the cart with the specified quantity and discount amount. If the product already exists in
        /// the cart, increases its quantity and discount amount accordingly.
        /// </summary>
        /// <remarks>If the specified product is already present in the cart, this method increments its
        /// quantity and discount amount rather than adding a duplicate entry.</remarks>
        /// <param name="product">The product to add to the cart. Cannot be null.</param>
        /// <param name="quantity">The quantity of the product to add. Must be greater than zero.</param>
        /// <param name="discountAmount">The discount amount to apply to the product. Must be zero or a positive value.</param>
        public void AddItem(Product product, decimal quantity, decimal discountAmount)
        {
            var existingItem = CartItems.FirstOrDefault(i => i.Product.PLU == product.PLU);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.DiscountAmount += discountAmount;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    DiscountAmount = discountAmount
                });
            }
        }

        /// <summary>
        /// Adds the specified product and quantity to the collection.
        /// </summary>
        /// <param name="product">The product to add. Cannot be null.</param>
        /// <param name="quantity">The quantity of the product to add. Must be a non-negative value.</param>
        public void AddItem(Product product, decimal quantity)
        {
            AddItem(product, quantity, 0);
        }
        public decimal TotalPrice => CartItems.Sum(i => i.TotalPrice); 

        public void Clear() => CartItems.Clear(); 
        public bool RemoveItem(string plu)
        {
            var item = CartItems.FirstOrDefault(i => i.Product.PLU == plu);

            if (item == null)
                return false;

            CartItems.Remove(item);
            return true;
        }

    }
}
