using System;
using System.Collections.Generic;
using System.Linq;
using CashRegisterSystem.Models;
using CashRegisterSystem.Core.Services;

namespace CashRegisterSystem.Core.CheckoutService
{
    internal class CartService
    {
        public void AddItem(Cart cart, Product product, decimal quantity, decimal discountAmount)
        {
            // Hämta korrekt pris efter kampanj
            decimal unitPrice = CampaignService.GetPrice(product);

            cart.AddItem(product, quantity, discountAmount, unitPrice);
        }

        public bool RemoveItem(Cart cart, string plu)
        {
            return cart.RemoveItem(plu);
        }

        public decimal CalculateTotal(Cart cart)
        {
            return cart.CartItems.Sum(item =>
                (item.UnitPrice * item.Quantity) - item.DiscountAmount);
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