namespace CashRegisterSystem.Controllers;

using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.Data.Interfaces;
using CashRegisterSystem.UI;

public static class CheckoutController
{
    public static void StartNewCustomer(List<Product> products)
    {
        var campaigns = CampaignRepository.GetCampaigns();

        Console.Clear();
        PLUDisplayUI.PrintPLUList(products);

        Cart cart = new Cart();

        while (true)
        {
            ShowPluInputPrompt();

            string? inputNewCustomer = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputNewCustomer))
                continue;

            string trimmed = inputNewCustomer.Trim().ToUpper();

            // PAY – avslutar köpet
            if (trimmed == "PAY")
            {
                bool continueShopping = CompletePayment(cart);

                if (!continueShopping)
                    return;

                cart.Clear();
                Console.Clear();
                PLUDisplayUI.PrintPLUList(products);
                continue;
            }

            // CANCEL – avbryter hela köpet
            if (trimmed == "CANCEL")
            {
                Console.WriteLine("Transaktionen avbröts / Transaction cancelled.");
                return;
            }

            // REMOVE – ta bort vara från kundvagnen 
            if (trimmed.StartsWith("DELETE") || trimmed.StartsWith("DEL"))
            {
                var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    Console.WriteLine("Fel format. Skriv: DEL [PLU] för att ta bort en vara.");
                    continue;
                }

                string pluToRemove = parts[1];

                bool removed = cart.RemoveItem(pluToRemove);

                if (removed)
                    Console.WriteLine($"\nVara med PLU {pluToRemove} borttagen från kundvagnen.");
                else
                    Console.WriteLine($"\nIngen vara med PLU {pluToRemove} hittades i kundvagnen.");

                Thread.Sleep(1000);
                Console.Clear();
                PLUDisplayUI.PrintPLUList(products);
                ShowCart(cart);
                continue;
            }

            // Försöker tolka PLU + mängd
            var parsed = ParseInput(inputNewCustomer, products);
            if (parsed is null)
                continue;

            var (product, quantity) = parsed.Value;

            decimal discountAmount = CalculateDiscount(product, quantity, campaigns);

            cart.AddItem(product, quantity, discountAmount);

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

            PLUDisplayUI.PrintPLUList(products);
            ShowCart(cart);
        }
    }

    private static (Product product, decimal quantity)? ParseInput(string input, List<Product> products)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
        {
            Console.WriteLine("Fel format. Skriv in PLU (mellanslag) antal/kg");
            return null;
        }

        string plu = parts[0];
        string quantityText = parts[1].Replace(".", ",");

        var product = products.FirstOrDefault(p => p.PLU == plu);
        if (product is null)
        {
            Console.WriteLine("Produkt hittades inte. Försök igen.");
            return null;
        }

        if (!decimal.TryParse(quantityText, out decimal quantity) || quantity <= 0)
        {
            Console.WriteLine("Ogiltigt antal/vikt.");
            return null;
        }

        return (product, quantity);
    }

    private static decimal CalculateDiscount(Product product, decimal quantity, List<Campaign> campaigns)
    {
        var validCampaigns = campaigns
            .Where(c => c.IsActive &&
                        c.StartDate <= DateTime.Today &&
                        c.EndDate >= DateTime.Today)
            .ToList();

        if (!validCampaigns.Any())
            return 0;

        var campaign = validCampaigns
            .FirstOrDefault(c => c.Name == product.Name);

        if (campaign == null)
            return 0;

        decimal totalPrice = product.Price * quantity;

        return totalPrice * (campaign.Discount / 100);
    }

    private static bool CompletePayment(Cart cart)
    {
        IReceiptWriter receiptWriter = new ReceiptFileWriter();

        receiptWriter.WriteReceipt(cart.CartItems);

        Console.WriteLine("\nTryck ENTER för nästa kund eller ESC för att gå tillbaka till menyn.");

        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape)
            return false;

        return true;
    }

    private static void ShowCart(Cart cart)
    {
        Console.WriteLine("------------------------------KUNDVAGN--------------------------------\n");

        decimal totalSum = 0;

        foreach (var item in cart.CartItems)
        {
            string unit = item.Product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";

            decimal total = item.Product.Price * item.Quantity - item.DiscountAmount;
            totalSum += total;

            Console.WriteLine($"{item.Product.Name,-25} á {item.Quantity,5} {unit,-3} {total,25:F2} kr");
        }
        Console.WriteLine("----------------------------------------------------------------------");
        Console.WriteLine($"{"",47}Total : {totalSum,8:F2} kr");
    }

    private static void ShowPluInputPrompt()
    {
        Console.WriteLine("----------------------------------------------------------------------");
        Console.WriteLine(" PLU [mellanslag] antal/vikt(kg)    |    (PAY / CANCEL / DELETE PLU) ");
        Console.WriteLine("----------------------------------------------------------------------\n");
    }
}
