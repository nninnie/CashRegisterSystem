using System;
using System.Collections.Generic;
using System.Threading;
using CashRegisterSystem.Core.CheckoutService;
using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers
{
    /// <summary>
    /// Handles the full checkout flow: parsing input, adding/removing items,
    /// applying discounts and completing payment.
    /// </summary>
    public class CheckoutController
    {
        private readonly InputParser _inputParser = new();
        private readonly DiscountService _discountService = new();
        private readonly PaymentService _paymentService = new();
        private readonly CartService _cartService = new();
        private readonly CommandParser _commandParser = new();

        public void StartNewCustomer(List<Product> products)
        {
            if (products == null)
                products = new List<Product>();

            Cart cart = new Cart();

            ShowCheckoutPluList(products);

            while (true)
            {
                CartUI.ShowCart(cart);
                CartUI.ShowPluInputPrompt();

                (Product product, decimal quantity)? parsed = null;

                while (parsed == null)
                {
                    string? input = InputService.ReadString("PLU och antal/vikt: ");
                    if (input == null)
                        return;   // ESC - gå tillbaka

                    string trimmed = input.Trim().ToUpper();


                    // PAY
                    if (_commandParser.IsPayCommand(trimmed))
                    {
                        bool continueShopping = _paymentService.CompletePayment(cart);

                        if (!continueShopping)
                            return;

                        _cartService.Clear(cart);
                        Console.Clear();
                        ShowCheckoutPluList(products);
                        break;
                    }

                    // CANCEL
                    if (_commandParser.IsCancelCommand(trimmed))
                    {
                        Console.WriteLine("Transaktionen avbröts");
                        return;
                    }

                    // DELETE
                    if (_commandParser.IsDeleteCommand(trimmed))
                    {
                        HandleDeleteCommand(trimmed, cart, products);
                        break;
                    }

                    // Försök tolka PLU + antal/vikt
                    parsed = _inputParser.Parse(input, products);

                    if (parsed == null)
                        ShowError("Fel format. Skriv: PLU [mellanslag] antal/vikt");
                }

                if (parsed == null)
                    continue;

                var (product, quantity) = parsed.Value;

                // Validering av antal/vikt
                if (!ValidateQuantity(product, quantity))
                    continue;

                // Rabatt
                decimal discountAmount = _discountService.CalculateDiscount(product, quantity);

                // Lägg till vara
                AddItemToCart(cart, product, quantity, discountAmount);

                // Rabattmeddelande
                if (discountAmount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Rabatt tillämpad: -{discountAmount:F2} kr");
                    Console.ResetColor();
                }

                Thread.Sleep(1200);
                Console.Clear();
                ShowCheckoutPluList(products);
            }
        }

        private void HandleDeleteCommand(string trimmed, Cart cart, List<Product> products)
        {
            string? pluToRemove = _commandParser.ExtractPluFromDelete(trimmed);

            if (pluToRemove is null)
            {
                ShowError("Fel format. Skriv: DEL [PLU]");
                return;
            }

            bool removed = _cartService.RemoveItem(cart, pluToRemove);

            if (removed)
                Console.WriteLine($"\nVara med PLU {pluToRemove} borttagen.");
            else
                Console.WriteLine($"\nIngen vara med PLU {pluToRemove} hittades.");

            Thread.Sleep(1000);
            Console.Clear();
            ShowCheckoutPluList(products);
            CartUI.ShowCart(cart);
        }

        private bool ValidateQuantity(Product product, decimal quantity)
        {
            if (product.PriceUnit == PriceUnit.Each)
            {
                if (quantity <= 0 || quantity % 1 != 0)
                {
                    ShowError("Styckvaror måste vara ett heltal (t.ex. 1, 2, 3).");
                    return false;
                }
            }
            else if (product.PriceUnit == PriceUnit.PerKilo)
            {
                if (quantity <= 0)
                {
                    ShowError("Vikt måste vara större än 0 (t.ex. 0.25, 1.5).");
                    return false;
                }
            }

            return true;
        }

        private void AddItemToCart(Cart cart, Product product, decimal quantity, decimal discountAmount)
        {
            if (product.PriceUnit == PriceUnit.PerKilo)
            {
                _cartService.AddItemByWeight(cart, product, quantity, discountAmount);
                Console.WriteLine($"\n{quantity:F2} kg av {product.Name} tillagd i kundvagnen.");
            }
            else
            {
                int qty = (int)quantity;
                _cartService.AddItem(cart, product, qty, discountAmount);
                Console.WriteLine($"\n{qty} st av {product.Name} tillagd i kundvagnen.");
            }
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
            Thread.Sleep(1200);
        }

        private void ShowCheckoutPluList(List<Product> products)
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("KASSA");
            ProductListUI.ShowCompactPLU(products);
        }

    }
}
