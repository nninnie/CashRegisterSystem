// Controller som hanterar hela kassaflödet för en kund
using CashRegisterSystem.Core;
using CashRegisterSystem.Data;

public static class CheckoutController
{
    public static void StartNewCustomer(List<Product> products)
    {
        Console.Clear();

        // Visar alla produkter i sorterad lista
        Console.WriteLine("=== KASSA ===\n");
        Console.WriteLine($"{" PLU",-6} {"Produkt",-11} {"Pris"}");
        Console.WriteLine("-----------------------------\n");

        foreach (var p in products.OrderBy(p => p.Name))
        {
            string type = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";
            Console.WriteLine($"{p.PLU} - {p.Name} - {p.Price} {type}");
        }

        // Skapar en ny varukorg för kunden
        Cart cart = new Cart();

        while (true)
        {
            Console.Write("\nPLU och antal/vikt (eller PAY/CANCEL): ");
            string? inputNewCustomer = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputNewCustomer))
                continue;

            string trimmed = inputNewCustomer.Trim().ToUpper();

            // ============================
            // PAY – avslutar köpet
            // ============================
            if (trimmed == "PAY")
            {
                IReceiptWriter receiptWriter = new ReceiptFileWriter();

                // Skriver kvitto till fil
                receiptWriter.WriteReceipt(cart.CartItems);

                // Skriver kvitto till skärmen
                Console.WriteLine("\n---KVITTO---");
                foreach (var item in cart.CartItems)
                {
                    Console.WriteLine($"{item.Product.Name} {item.Quantity} x {item.Product.Price} = {item.TotalPrice} kr");
                }

                Console.WriteLine($"TOTALT: {cart.TotalPrice} kr");
                Console.WriteLine("--------------\n");

                Console.WriteLine("Betalning genomförd.");
                Console.WriteLine("Kvitto skapat. Tack för ditt köp och trevlig dag!");

                // Tömmer varukorgen inför nästa kund
                cart.Clear();
                continue;
            }

            // ============================
            // CANCEL – avbryter hela köpet
            // ============================
            if (trimmed == "CANCEL")
            {
                Console.WriteLine("Transaktionen avbröts / Transaction cancelled.");
                return;
            }

            // ============================
            // Hantering av PLU + mängd
            // ============================
            string[] parts = inputNewCustomer.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                Console.WriteLine("Fel format. Skriv in PLU (mellanslag) antal/kg");
                continue;
            }

            string plu = parts[0];
            string quantityText = parts[1];

            // Hittar produkten baserat på PLU
            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product is null)
            {
                Console.WriteLine("Produkt hittades inte. Försök igen.");
                continue;
            }

            // Tillåter både punkt och komma som decimaltecken
            quantityText = quantityText.Replace(".", ",");

            // Validerar mängden
            if (!decimal.TryParse(quantityText, out decimal quantity) || quantity <= 0)
            {
                Console.WriteLine("Ogiltigt antal/vikt.");
                continue;
            }

            // Lägger till varan i varukorgen
            cart.AddItem(product, quantity);

            string unitText = product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
            Console.WriteLine($"{quantity} {unitText} av {product.Name} tillagd i kundvagnen.");
        }
    }
}
