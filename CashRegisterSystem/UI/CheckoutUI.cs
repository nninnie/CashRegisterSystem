using System;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class CheckoutUI
    {
        public static void ShowCart(Cart cart)
        {
            Console.WriteLine("------------------------------KUNDVAGN--------------------------------\n");

            decimal totalSum = 0;

            foreach (var item in cart.CartItems)
            {
                string unit = item.Product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                decimal total = item.Product.Price * item.Quantity - item.DiscountAmount;
                totalSum += total;

                Console.WriteLine(
                    $"{item.Product.Name,-16}" +
                    $"{item.Quantity,18}" +
                    $"{unit,4}" +
                    $"{total,29:F2} kr");
            }

            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine($"{"",51}Total : {totalSum,8:F2} kr");
        }

        public static void ShowPluInputPrompt()
        {
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine(" PLU [mellanslag] antal/vikt(kg)    |    (PAY / CANCEL / DELETE PLU) ");
            Console.WriteLine("----------------------------------------------------------------------\n");
        }
    }
}
