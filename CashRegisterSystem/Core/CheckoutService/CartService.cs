using System;
using System.Collections.Generic;
using System.Linq;
using CashRegisterSystem.Models;
using CashRegisterSystem.Core.Services;

namespace CashRegisterSystem.Core.CheckoutService
{
    /// <summary>
    /// Handles adding, removing and calculating totals for cart items.
    /// Ensures safe operations and prevents crashes from invalid input.
    /// </summary>
    internal class CartService
    {
        public void AddItem(Cart cart, Product product, int quantity, decimal discountAmount)
        {
            if (cart == null || product == null || quantity <= 0)
                return;

            decimal unitPrice = CampaignService.GetPrice(product);
            cart.AddItem(product, quantity, discountAmount, unitPrice);
        }

        public void AddItemByWeight(Cart cart, Product product, decimal weight, decimal discountAmount)
        {
            if (cart == null || product == null || weight <= 0)
                return;

            decimal unitPrice = CampaignService.GetPrice(product);
            cart.AddItemByWeight(product, weight, discountAmount, unitPrice);
        }

        public bool RemoveItem(Cart cart, string plu)
        {
            if (cart == null || string.IsNullOrWhiteSpace(plu))
                return false;

            return cart.RemoveItem(plu);
        }

        public decimal CalculateTotal(Cart cart)
        {
            if (cart == null || cart.CartItems.Count == 0)
                return 0;

            return cart.CartItems.Sum(item => item.FinalPrice);
        }

        public IEnumerable<CartItem> GetItems(Cart cart)
        {
            return cart?.CartItems ?? Enumerable.Empty<CartItem>();
        }

        public void Clear(Cart cart)
        {
            cart?.Clear();
        }
    }
}
