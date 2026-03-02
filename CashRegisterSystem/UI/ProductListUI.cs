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

            // Sort alphabetically by product name
            var sortedProducts = products
                .OrderBy(product => product.Name)
                .ToList();

            foreach (var product in sortedProducts)
            {
                PrintProduct(product);
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
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

        /// <summary>
        /// Shows a compact 3-column PLU list including price.
        /// Used in Edit Product (admin), not in checkout.
        /// </summary>
        public static void ShowCompactWithPrice(List<Product> products)
        {
            var sorted = products.OrderBy(p => p.Name).ToList();

            int colWidth = 38;
            int pluWidth = 6;
            int nameWidth = 14;
            int priceWidth = 10;

            int itemsPerRow = 2;
            int rowCount = (int)Math.Ceiling(sorted.Count / (double)itemsPerRow);

            for (int i = 0; i < rowCount; i++)
            {
                string row = "";

                for (int col = 0; col < itemsPerRow; col++)
                {
                    int index = i + col * rowCount;
                    if (index >= sorted.Count)
                    {
                        row += "".PadRight(colWidth);
                        continue;
                    }

                    var p = sorted[index];

                    string plu = (p.PLU ?? "").PadRight(pluWidth);
                    string name = (p.Name ?? "").PadRight(nameWidth);
                    string unit = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";
                    string price = $"{p.Price:0.00} {unit}".PadRight(priceWidth);

                    // ⭐ Kampanjtext beroende på typ
                    string campaignText = "";

                    if (p.CampaignIsActive)
                    {
                        // FAST KAMPANJPRIS
                        if (p.CampaignPrice > 0)
                        {
                            campaignText = $" (KAMPANJ: {p.CampaignPrice:0.00} kr)";
                        }
                        // PROCENTKAMPANJ
                        else if (p.CampaignPercent > 0)
                        {
                            decimal newPrice = p.Price * (1 - (p.CampaignPercent / 100));
                            campaignText = $" (KAMPANJ: {newPrice:0.00} kr)";
                        }
                        // MULTIKÖP
                        else if (p.MultiBuyQuantity > 0 && p.MultiBuyPrice > 0)
                        {
                            campaignText = " (KAMPANJ MULTI)";
                        }
                    }

                    string colText = $"{plu} {name} {price}{campaignText}";
                    row += colText.PadRight(colWidth);
                }

                Console.WriteLine(row);
            }

            Console.WriteLine();
        }

    }
}

