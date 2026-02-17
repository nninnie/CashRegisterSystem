using CashRegisterSystem.Core;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides a compact multi-column PLU list layout for the cashier interface.
    /// Automatically distributes products across up to four columns depending on
    /// the number of items, keeping the view clean and easy to scan.
    /// </summary>
    public static class PLUDisplayUI
    {
        public static void PrintPLUList(List<Product> products, int maxColumns = 4)
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("KASSA");

            int colWidth = 22;
            int columns = Math.Min(maxColumns, (int)Math.Ceiling(products.Count / 8.0));

            int rows = (int)Math.Ceiling(products.Count / (double)columns);
            List<List<Product>> cols = new();

            for (int c = 0; c < columns; c++)
            {
                cols.Add(products.Skip(c * rows).Take(rows).ToList());
            }

            for (int r = 0; r < rows; r++)
            {
                string line = "";

                for (int c = 0; c < columns; c++)
                {
                    if (r < cols[c].Count)
                    {
                        var p = cols[c][r];
                        line += $"{p.PLU} {p.Name}".PadRight(colWidth);
                    }
                    else
                    {
                        line += "".PadRight(colWidth);
                    }
                }

                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }
}
