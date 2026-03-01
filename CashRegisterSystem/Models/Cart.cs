using System;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Models
{
    public class Cart
    {
        public List<CartItem> CartItems { get; } = new();

        /// <summary>
        /// Adds a product to the cart with full control over quantity, discount and unit price.
        /// </summary>
        public void AddItem(Product product, decimal quantity, decimal discountAmount, decimal unitPrice)
        {
            var existingCartItem = CartItems.FirstOrDefault(item => item.Product.PLU == product.PLU);

            if (existingCartItem != null)
            {
                // Increase quantity and discount on existing item
                existingCartItem.Quantity += quantity;
                existingCartItem.DiscountAmount += discountAmount;

                // UnitPrice is intentionally NOT updated here.
                // A product keeps the same price it had when first added.
                return;
            }

            CartItems.Add(new CartItem
            {
                Product = product,
                Quantity = quantity,
                DiscountAmount = discountAmount,
                UnitPrice = unitPrice
            });
        }

        /// <summary>
        /// Adds a product without discount (fallback).
        /// </summary>
        public void AddItem(Product product, decimal quantity)
        {
            AddItem(product, quantity, 0, product.Price);
        }

        /// <summary>
        /// Total price before discount.
        /// </summary>
        public decimal TotalPrice => CartItems.Sum(item => item.TotalPrice);

        /// <summary>
        /// Clears the entire cart.
        /// </summary>
        public void Clear()
        {
            CartItems.Clear();
        }

        /// <summary>
        /// Removes a product from the cart by PLU.
        /// </summary>
        public bool RemoveItem(string plu)
        {
            var cartItem = CartItems.FirstOrDefault(item => item.Product.PLU == plu);

            if (cartItem == null)
                return false;

            CartItems.Remove(cartItem);
            return true;
        }
    }
}