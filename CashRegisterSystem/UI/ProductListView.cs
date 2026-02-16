using CashRegisterSystem.Core;

namespace CashRegisterSystem.UI
{
    public static class ProductListView
    {
        public static void ShowProducts(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                 PLU-LISTA");
            Console.ResetColor();
            Console.WriteLine("========================================");
            Console.WriteLine();


            // Loopar igenom produktlistan och skriver ut varje produkt.
            // .OrderBy(p => p.Name) sorterar listan alfabetiskt efter produktnamn.
            foreach (var p in products.OrderBy(p => p.Name))
            {
                // Visar rätt enhet beroende på om priset är per kilo eller per styck
                string type = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

                // OBS: p.PLU är korrekt (inte p.Id)
                Console.WriteLine($"PLU: {p.PLU} - {p.Name} - {p.Price} {type}");
            }

            // Väntar på att användaren trycker en tangent innan vi går tillbaka
            Console.WriteLine("\nTryck valfri tangent för att återgå till menyn...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
