using CashRegisterSystem.Core;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays a formatted product list in a receipt‑style layout,
    /// including PLU, product name and price information.
    /// </summary>
    public static class ProductListView
    {
        /// <summary>
        /// Shows all products in a clean, aligned table.
        /// Products are sorted alphabetically for easier scanning.
        /// </summary>
        public static void ShowProductList(List<Product> products)
        {
            BannerUI.PrintFruitHeader("PRODUKTLISTA");

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($" {"PLU",-6} {"Produkt",-20} {"Pris"}");
            Console.WriteLine("-----------------------------------------");

            foreach (var p in products.OrderBy(p => p.Name))
            {
                string type = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";
                string price = $"{p.Price} {type}";

                Console.WriteLine($" {p.PLU,-6} {p.Name,-20} {price}");
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("\nTryck valfri tangent för att återgå till menyn...");
            Console.ReadKey();
        }
    }
}
