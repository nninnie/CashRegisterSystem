using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public static class CheckoutController
    {
        public static void StartNewCustomer(List<Product> products)
        {

            Console.Clear();

            Console.WriteLine("=== KASSA ===\n");
            Console.WriteLine($"{" PLU",-6} {"Produkt",-11} {"Pris"}");
            Console.WriteLine("-----------------------------\n");

            foreach (var p in products.OrderBy(p => p.Name))
            {
                string type = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";
                Console.WriteLine($"{p.Id} - {p.Name} - {p.Price} {type}");
            }

            List<(Product Product, decimal Quantity)> cart = new();

            while (true)
            {
                Console.Write("\nPLU och antal/vikt (eller PAY/CANCEL): ");
                string? inputNewCustomer = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputNewCustomer))
                    continue;

                string trimmed = inputNewCustomer.Trim().ToUpper();

                if (trimmed == "PAY")
                {
                    IReceiptWriter receiptWriter = new ReceiptWriter();
                    receiptWriter.WriteReceipt(cart);
                    return;
                }
                if (trimmed == "CANCEL")
                {
                    Console.WriteLine("Transaktionen avbröts / Transaction cancelled.");
                    return;
                }

                string[] parts = inputNewCustomer.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    Console.WriteLine("Fel format. Skriv in PLU (melanslag) antal/kg");
                    continue;
                }


                string plu = parts[0];
                string quantityText = parts[1];

                var product = products.FirstOrDefault(p => p.Id == plu);

                if (product is null)
                {
                    Console.WriteLine("Produkt hittades inte. Försök igen.");
                    continue;
                }
                if (!decimal.TryParse(quantityText, out decimal quantity) || quantity <= 0)
                {
                    Console.WriteLine("Ogiltigt antal/vikt.");
                    continue;
                }

                cart.Add((product, quantity));

                string unitText = product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                Console.WriteLine($"{quantity} {unitText} av {product.Name} tillagd i kundvagnen.");
            }
        }
    }
}
