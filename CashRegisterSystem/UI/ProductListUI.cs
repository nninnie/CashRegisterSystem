using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays the PLU product list in a clean, scalable and safe way.
    /// Uses ConsoleLayout for consistent formatting across the application.
    /// </summary>
    public static class ProductListUI
    {
        /// <summary>
        /// Shows the full PLU list sorted alphabetically by product name.
        /// </summary>
        public static void ShowProductList(List<Product> products)
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("PLU-LISTA");

            Console.WriteLine();
            ConsoleLayout.WriteDivider();

            // Sort alphabetically by product name
            var sortedProducts = products
                .OrderBy(product => product.Name)
                .ToList();

            foreach (var product in sortedProducts)
            {
                PrintProduct(product);
            }

            ConsoleLayout.WriteDivider();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nÅtergå till menyn");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        /// <summary>
        /// Prints a single product row with PLU, name, price and optional campaign price.
        /// Automatically truncates long names and keeps columns aligned.
        /// </summary>
        private static void PrintProduct(Product product)
        {
            // Null-safety to prevent crashes
            string safePLU = product.PLU ?? "----";
            string safeName = product.Name ?? "(saknas)";

            // Truncate long names for clean layout
            if (safeName.Length > 22)
                safeName = safeName.Substring(0, 19) + "...";

            string unitLabel = product.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

            bool hasActiveCampaign =
                product.CampaignPrice > 0 &&
                product.CampaignPrice < product.Price;

            decimal displayPrice = hasActiveCampaign
                ? product.CampaignPrice
                : product.Price;

            // Build columns using ConsoleLayout for consistency
            Console.ForegroundColor = ConsoleColor.White;

            ConsoleLayout.WriteColumns(
                (safePLU, 8),
                (safeName, 22),
                ($"{displayPrice:0.00} {unitLabel}", 12)
            );

            // Print original price if campaign is active
            if (hasActiveCampaign)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{"",42}(Ord: {product.Price:0.00})");
            }

            Console.ResetColor();
        }
    }
}
