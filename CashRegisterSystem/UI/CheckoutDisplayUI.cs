using CashRegisterSystem.Models;
using System;
namespace CashRegisterSystem.UI
{
    public static class CheckoutDisplayUI
    {
        public static void PrintPLUList(List<Product> products, int maxColumns = 4)
        {
            Console.Clear();

            // Centered header instead of emojis
            ConsoleLayout.WriteCentered("KASSA");
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------------------");

            int columnWidth = 25;
            int columnCount = Math.Min(maxColumns, 3);
            int rowCount = (int)Math.Ceiling(products.Count / (double)columnCount);

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

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                string rowText = "";

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    if (rowIndex < productColumns[columnIndex].Count)
                    {
                        Product product = productColumns[columnIndex][rowIndex];
                        rowText += $"{product.PLU} {product.Name}".PadRight(columnWidth);
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