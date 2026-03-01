using System;
using System.Collections.Generic;
using CashRegisterSystem.Core.CheckoutService;
using CashRegisterSystem.Data;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using CashRegisterSystem.Core.Services;

namespace CashRegisterSystem.Controllers
{
    public class CheckoutController
    {
        private readonly InputParser _inputParser = new();
        private readonly DiscountService _discountService = new();
        private readonly PaymentService _paymentService = new();
        private readonly CartService _cartService = new();
        private readonly CommandParser _commandParser = new();

        public void StartNewCustomer(List<Product> products)
        {
            var campaigns = CampaignFileLoader.GetCampaigns();
            Cart cart = new Cart();

            Console.Clear();
            CheckoutDisplayUI.PrintPLUList(products);

            while (true)
            {
                CartUI.ShowCart(cart);
                CartUI.ShowPluInputPrompt();

                (Product product, decimal quantity)? parsed = null;

                while (parsed == null)
                {
                    string input = InputService.ReadString("PLU och antal/vikt: ");

                    string trimmed = input.Trim().ToUpper();

                    // PAY
                    if (_commandParser.IsPayCommand(trimmed))
                    {
                        bool continueShopping = _paymentService.CompletePayment(cart);

                        if (!continueShopping)
                            return;

                        _cartService.Clear(cart);
                        Console.Clear();
                        CheckoutDisplayUI.PrintPLUList(products);
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
                        string? pluToRemove = _commandParser.ExtractPluFromDelete(trimmed);

                        if (pluToRemove is null)
                        {
                            Console.WriteLine("Fel format. Skriv: DEL [PLU] för att ta bort en vara.");
                            continue;
                        }

                        bool removed = _cartService.RemoveItem(cart, pluToRemove);

                        if (removed)
                            Console.WriteLine($"\nVara med PLU {pluToRemove} borttagen från kundvagnen.");
                        else
                            Console.WriteLine($"\nIngen vara med PLU {pluToRemove} hittades i kundvagnen.");

                        Thread.Sleep(1000);
                        Console.Clear();
                        CheckoutDisplayUI.PrintPLUList(products);
                        CartUI.ShowCart(cart);
                        break;
                    }

                    // Försök tolka PLU
                    parsed = _inputParser.Parse(input, products);
                }

                if (parsed == null)
                    continue;

                var (product, quantity) = parsed.Value;

                decimal discountAmount = _discountService.CalculateDiscount(product, quantity, campaigns);

                _cartService.AddItem(cart, product, quantity, discountAmount);

                string unitText = product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                Console.WriteLine($"\n{quantity} {unitText} av {product.Name} tillagd i kundvagnen.");

                if (discountAmount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Rabatt tillämpad: -{discountAmount:F2} kr");
                    Console.ResetColor();
                }

                Thread.Sleep(1200);
                Console.Clear();
                CheckoutDisplayUI.PrintPLUList(products);
            }
        }
    }
}
