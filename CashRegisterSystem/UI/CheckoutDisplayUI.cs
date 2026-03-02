using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Displays the PLU list inside the checkout screen.
    /// The layout is column-based and scales depending on the number of products.
    /// </summary>
    public static class CheckoutDisplayUI
    {
        /// <summary>
        /// Prints the PLU list in multiple columns inside the checkout view.
        /// </summary>
        public static void PrintPLUList(List<Product> products, int maxColumns = 4)
        {
            Console.Clear();

            BannerUI.ShowLogo();
            BannerUI.PrintHeader("KASSA");

            // Limit the number of columns for readability inside the checkout screen
            int columnCount = Math.Min(maxColumns, 3);

            // Width of each column (kept simple for readability)
            int columnWidth = 25;

            // Calculate how many rows are needed
            int rowCount = (int)Math.Ceiling(products.Count / (double)columnCount);

            // Prepare column lists
            List<List<Product>> productColumns = new();

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                productColumns.Add(
                    products
                        .Skip(columnIndex * rowCount)
                        .Take(rowCount)
                        .ToList()
                );
            }

            // Print rows
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                string rowText = "";

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    if (rowIndex < productColumns[columnIndex].Count)
                    {
                        Product product = productColumns[columnIndex][rowIndex];

                        // Null-safety
                        string safePLU = product.PLU ?? "----";
                        string safeName = product.Name ?? "(saknas)";

                        // Truncate long names
                        if (safeName.Length > 18)
                            safeName = safeName.Substring(0, 15) + "...";

                        rowText += $"{safePLU} {safeName}".PadRight(columnWidth);
                    }
                    else
                    {
                        rowText += "".PadRight(columnWidth);
                    }
                }

                Console.WriteLine(rowText);
            }

            Console.WriteLine();
        }
    }
}
