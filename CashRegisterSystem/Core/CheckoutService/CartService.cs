using System;
using System.Collections.Generic;
using System.Linq;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Core.CheckoutService
{
    internal class CartService
    {
        /// <summary>
        /// Adds an item to the cart using the correct unit price (campaign or regular).
        /// </summary>
        public void AddItem(Cart cart, Product product, decimal quantity, decimal discountAmount)
        {
            // Hämta kampanjpris eller ordinarie pris
            decimal unitPrice = ProductUI.GetPrice(product);

            // Skicka in priset till Cart
            cart.AddItem(product, quantity, discountAmount, unitPrice);
        }

        /// <summary>
        /// Removes an item from the cart by PLU.
        /// </summary>
        public bool RemoveItem(Cart cart, string plu)
        {
            return cart.RemoveItem(plu);
        }

        /// <summary>
        /// Calculates the total price of the cart using unit prices and discounts.
        /// </summary>
        public decimal CalculateTotal(Cart cart)
        {
            return cart.CartItems.Sum(item =>
                (item.UnitPrice * item.Quantity) - item.DiscountAmount);
        }

        /// <summary>
        /// Returns all items in the cart.
        /// </summary>
        public IEnumerable<CartItem> GetItems(Cart cart)
        {
            return cart.CartItems;
        }

        /// <summary>
        /// Clears the cart.
        /// </summary>
        public void Clear(Cart cart)
        {
            cart.Clear();
        }
    }
}