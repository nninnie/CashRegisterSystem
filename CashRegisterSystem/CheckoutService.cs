using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public static class CheckoutService
    {
        public static void StartNewCustomer(List<ProductModel> products)
        {

            Console.Clear();

            Console.WriteLine("--- SJÄLVSCANNING ---\n");
            Console.WriteLine($"{"PLU",-6} {"Produkt",-20} {"Pris"}");

            foreach (var product in products)
            {
                string unit = product.PriceUnit == PriceUnit.PerKilo ? "/kg" : "/st";
                Console.WriteLine($"{product.Id,-6} {product.Name,-20} {product.Price:C}");
            }

            Console.WriteLine("Vill du scaanna varor, pay, eller cancele? ");

            List<(ProductModel Product, decimal Quantity)> cart = new();


            while (true)
            {
                Console.WriteLine("PLU: ");
                
                string? usersInput = Console.ReadLine();


                if (string.IsNullOrEmpty(usersInput))
                    continue;


                if (usersInput.Trim().ToUpper() == "PAY")
                {
                    WriteReceipt(cart);
                    return;
                }
                if (usersInput.Trim().ToUpper() == "CANCEL")
                {
                    Console.WriteLine("Transaction cancelled.");
                    return;
                }
                var product = products.FirstOrDefault(p => p.Id == usersInput.Trim());  

                if (product is null)
                {
                    Console.WriteLine("Produkt hittades inte. Försök igen.");
                    continue;
                }


                if (product.PriceUnit == PriceUnit.PerKilo)
                {
                    Console.WriteLine("Ange vikt i kilo: ");
                    string? weightInput = Console.ReadLine();

                    if (!decimal.TryParse(weightInput, out decimal weight) || weight <= 0)
                    {
                        Console.WriteLine("Ogiltig vikt. Försök igen.");
                        continue;
                    }

                    cart.Add((product, weight));
                    Console.WriteLine($"{weight} kg av {product.Name} tillagd i kundvagnen.");
                    continue;
                }

                    Console.Write("Ange antal: ");
                    string? quantityInput = Console.ReadLine();

                    if (!decimal.TryParse(quantityInput, out decimal quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Ogiltigt antal. Försök igen.");
                        continue;
                    }

                cart.Add((product, quantity));
                    Console.WriteLine($"{quantity} st av {product.Name} tillagd i kundvagnen.");
                }
            }
       

                

        private static void WriteReceipt(List<(ProductModel Product, decimal Quantity)> cart)
        {
            /// KOD
        }
    }
}
