using CashRegisterSystem.Core;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays a formatted list of all products, including PLU, name, price and unit.
    /// </summary>
    public static class ProductListView
    {
        /// <summary>
        /// Shows the product list in alphabetical order and waits for user input before returning.
        /// </summary>
        /// <param name="products">The list of products to display.</param>
        public static void ShowProducts(List<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            Console.Clear();
            Console.WriteLine("========================================");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                 PLU-LISTA");
            Console.ResetColor();
            Console.WriteLine("========================================\n");

            foreach (var p in products.OrderBy(p => p.Name))
            {
                string type = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";
                Console.WriteLine($"PLU: {p.PLU} - {p.Name} - {p.Price} {type}");
            }

            Console.WriteLine("\nTryck valfri tangent för att återgå till menyn...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
