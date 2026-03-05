using System;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Models
{
    /// <summary>
    /// Represents the shopping cart. Stores cart items and
    /// provides methods for adding, removing and clearing items.
    /// Handles both quantity-based and weight-based products.
    /// </summary>
    public class Cart
    {
        public List<CartItem> CartItems { get; } = new();
        public void AddItem(Product product, int quantity, decimal discountAmount, decimal unitPrice)
        {
            if (product == null)
                return;

            if (quantity < 0)
                quantity = 0;

            if (discountAmount < 0)
                discountAmount = 0;

            var existingCartItem = CartItems.FirstOrDefault(item => item.Product.PLU == product.PLU);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
                existingCartItem.DiscountAmount += discountAmount;
                return;
            }

            CartItems.Add(new CartItem
            {
                Product = product,
                Quantity = quantity,
                Weight = 0,
                DiscountAmount = discountAmount,
                UnitPrice = unitPrice
            });
        }
        public void AddItemByWeight(Product product, decimal weight, decimal discountAmount, decimal unitPrice)
        {
            if (product == null)
                return;

            if (weight < 0)
                weight = 0;

            if (discountAmount < 0)
                discountAmount = 0;

            var existingCartItem = CartItems.FirstOrDefault(item => item.Product.PLU == product.PLU);

            if (existingCartItem != null)
            {
                existingCartItem.Weight += weight;
                existingCartItem.DiscountAmount += discountAmount;
                return;
            }

            CartItems.Add(new CartItem
            {
                Product = product,
                Quantity = 0,
                Weight = weight,
                DiscountAmount = discountAmount,
                UnitPrice = unitPrice
            });
        }

        public void AddItem(Product product, int quantity)
        {
            AddItem(product, quantity, 0, product.Price);
        }

        public decimal TotalPrice => CartItems.Sum(item => item.TotalPrice);

        public void Clear() => CartItems.Clear();

        public bool RemoveItem(string plu)
        {
            if (string.IsNullOrWhiteSpace(plu))
                return false;

            var cartItem = CartItems.FirstOrDefault(item => item.Product.PLU == plu);

            if (cartItem == null)
                return false;

            CartItems.Remove(cartItem);
            return true;
        }
    }
}
