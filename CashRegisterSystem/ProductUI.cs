using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public static class ProductUI
    {
        public static void ShowProducts(List<ProductModel> products)
        {
            Console.WriteLine("\n=== Frukt & Grönt ===");


            //loopar igenom produktlistan, skriver ut varje p = produkt
            // Vi använder .OrderBy(p => p.Name) för att sortera efter namn innan vi loopar
            foreach (var p in products.OrderBy(p => p.Name))
            {
                string type = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";
                Console.WriteLine($"PLU: {p.Id} - {p.Name} - {p.Price} {type}");
            }



            Console.WriteLine("\nTryck valfri tangent för att återgå till menyn..."); 
            Console.ReadKey();
            Console.Clear();
        }
    }
}
