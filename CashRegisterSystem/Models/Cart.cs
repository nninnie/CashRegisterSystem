using System;
using System.Collections.Generic;
using System.Linq;

namespace CashRegisterSystem.Models
{
    public class Cart
    {
        public List<CartItem> CartItems { get; } = new();

        public void AddItem(Product product, decimal quantity, decimal discountAmount, decimal unitPrice)
        {
            var existingItem = CartItems.FirstOrDefault(i => i.Product.PLU == product.PLU);

            if (existingItem != null)
            {
                // Lägg ihop mängd och rabatt
                existingItem.Quantity += quantity;
                existingItem.DiscountAmount += discountAmount;

                // Viktigt: använd samma unit price som första gången
                // (kampanjpris ska inte ändras mitt i en kundvagn)
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    DiscountAmount = discountAmount,
                    UnitPrice = unitPrice
                });
            }
        }

        /// <summary>
        /// Adds a product without discount (fallback).
        /// </summary>
        public void AddItem(Product product, decimal quantity)
        {
            AddItem(product, quantity, 0, product.Price);
        }

        /// <summary>
        /// Totalpris före rabatt.
        /// </summary>
        public decimal TotalPrice => CartItems.Sum(i => i.TotalPrice);

        /// <summary>
        /// Rensar kundvagnen.
        /// </summary>
        public void Clear() => CartItems.Clear();

        /// <summary>
        /// Removes an item by PLU.
        /// </summary>
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