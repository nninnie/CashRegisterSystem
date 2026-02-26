using System;
using System.Collections.Generic;
using System.Linq;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.CheckoutService
{
    internal class CartService
    {
        public void AddItem(Cart cart, Product product, decimal quantity, decimal discountAmount)
        {
            cart.AddItem(product, quantity, discountAmount);
        }

        public bool RemoveItem(Cart cart, string plu)
        {
            return cart.RemoveItem(plu);
        }

        public decimal CalculateTotal(Cart cart)
        {
            return cart.CartItems.Sum(item =>
                (item.Product.Price * item.Quantity) - item.DiscountAmount);
        }

        public IEnumerable<CartItem> GetItems(Cart cart)
        {
            return cart.CartItems;
        }

        public void Clear(Cart cart)
        {
            cart.Clear();
        }
    }
}
