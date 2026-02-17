namespace CashRegisterSystem.Controllers;

using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;

/// <summary>
/// Manages the checkout flow for customer transactions in the cash register system.
/// </summary>
/// <remarks>
/// This controller handles scanning of products, applying discounts, managing the shopping cart,
/// and completing payments. It interacts with the console to guide the cashier through each step
/// of the transaction and coordinates receipt generation and cart clearing after payment.
/// </remarks>
public static class CheckoutController
{
    /// <summary>
    /// Starts a new customer transaction, allowing products to be added to a cart and handling payment or cancellation
    /// commands.
    /// </summary>
    /// <remarks>This method interacts with the console to receive input for product selection, quantity, and
    /// transaction commands. The transaction continues until the user enters the PAY or CANCEL command. Discounts are
    /// applied automatically based on active campaigns. This method is intended for use in interactive console
    /// applications and blocks until the transaction is completed or cancelled.</remarks>
    /// <param name="products">The list of available products that can be selected and added to the customer's cart. Cannot be null.</param>
    public static void StartNewCustomer(List<Product> products)
    {
        var campaigns = CampaignRepository.GetCampaigns();

        Console.Clear();
        PLUDisplayUI.PrintPLUList(products);

        Cart cart = new Cart();

        while (true)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.Write("\nPLU [mellanslag] antal/vikt    |    (PAY / CANCEL) : ");
            string? inputNewCustomer = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputNewCustomer))
                continue;

            string trimmed = inputNewCustomer.Trim().ToUpper();

            // PAY – avslutar köpet
            if (trimmed == "PAY")
            {
                bool continueShopping = CompletePayment(cart);

                if (!continueShopping)
                    return; // tillbaka till menyn

                cart.Clear(); // töm kundvagn för nästa kund
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

            // Försöker tolka PLU + mängd
            var parsed = ParseInput(inputNewCustomer, products);
            if (parsed is null)
                continue;

            var (product, quantity) = parsed.Value;

            // Beräknar rabatt baserat på aktiva kampanjer
            decimal discountAmount = CalculateDiscount(product, quantity, campaigns);

            // Lägger till varan i kundvagnen
            cart.AddItem(product, quantity, discountAmount);

            // Feedback till kassören
            string unitText = product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
            Console.WriteLine($"{quantity} {unitText} av {product.Name} tillagd i kundvagnen.");

            if (discountAmount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Rabatt tillämpad: -{discountAmount:F2} kr");
                Console.ResetColor();
            }


        }
    }

    /// <summary>
    /// Parses a user input string to extract a product and its associated quantity from a list of available products.
    /// </summary>
    /// <param name="input">The input string containing the product PLU code and quantity, separated by a space. The quantity may use either
    /// a period or comma as the decimal separator.</param>
    /// <param name="products">The list of available products to search for a matching PLU code.</param>
    /// <returns>A tuple containing the matching product and the parsed quantity if the input is valid and a matching product is
    /// found; otherwise, null.</returns>
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

    /// <summary>
    /// Calculates the discount amount for a product based on the specified quantity and applicable campaigns.
    /// </summary>
    /// <param name="product">The product for which the discount is to be calculated. Must not be null.</param>
    /// <param name="quantity">The quantity of the product to be considered when calculating the discount. Must be a non-negative value.</param>
    /// <param name="campaigns">A list of campaigns to evaluate for discount eligibility. Only active campaigns within their valid date range
    /// are considered.</param>
    /// <returns>The calculated discount amount as a decimal value. Returns 0 if no applicable campaign is found.</returns>
    private static decimal CalculateDiscount(Product product, decimal quantity, List<Campaign> campaigns)
    {
        var validCampaigns = campaigns
            .Where(c => c.IsActive &&
                        c.StartDate <= DateTime.Today &&
                        c.EndDate >= DateTime.Today)
            .ToList();

        if (!validCampaigns.Any())
            return 0;

        // Hitta kampanj för just denna produkt
        var campaign = validCampaigns
            .FirstOrDefault(c => c.Name == product.Name);

        // Ingen kampanj för produkten
        if (campaign == null)
            return 0;

        decimal totalPrice = product.Price * quantity;

        return totalPrice * (campaign.Discount / 100);
    }

    /// <summary>
    /// Finalizes the payment by generating and displaying the receipt, and waits for the cashier's
    /// input to determine whether to start a new customer or return to the main menu.
    /// </summary>
    /// <param name="cart">The cart containing the purchased items.</param>
    /// <returns>
    /// True if the cashier chooses to continue with a new customer; false if returning to the main menu.
    /// </returns>
    private static bool CompletePayment(Cart cart)

    {
        IReceiptWriter receiptWriter = new ReceiptFileWriter();

        // Visa och spara kvittot
        receiptWriter.WriteReceipt(cart.CartItems);

        Console.WriteLine("\nTryck ENTER för nästa kund eller ESC för att gå tillbaka till menyn.");

        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape)
            return false; 

        return true; // fortsätt till nästa kund
    }
}
