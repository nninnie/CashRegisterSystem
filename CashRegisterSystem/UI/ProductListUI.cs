using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// UI helper for displaying PLU-based product lists.
    /// Contains one standard admin list and one compact cashier list.
    /// </summary>
    public static class ProductListUI
    {
        /// <summary>
        /// Standard product list for admin and main menu.
        /// Displays one row per product in a table layout.
        /// </summary>
        public static void ShowProductList(List<Product> products)
        {
            Console.Clear();
            BannerUI.ShowLogo();

            BannerUI.PrintHeader("PRODUKTLISTA");

            // Column widths for clear table layout
            int colPLU = 6;
            int colName = 15;
            int colUnit = 6;
            int colPrice = 10;
            int colCampaign = 12;
            int colType = 8;

            // Header row
            Console.WriteLine(
                "PLU".PadRight(colPLU) + " | " +
                "Produkt".PadRight(colName) + " | " +
                "Enhet".PadRight(colUnit) + " | " +
                "Ord.pris".PadRight(colPrice) + " | " +
                "Kampanj".PadRight(colCampaign) + " | " +
                "Typ".PadRight(colType)
            );

            Console.WriteLine(new string('-', 70));

            // Product rows
            foreach (var p in products.OrderBy(p => p.Name))
            {
                string unit = p.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                string ordPrice = $"{p.Price:0.00}".PadRight(colPrice);

                // Added: campaign value + campaign type
                string kampValue;
                string kampType;

                if (!p.CampaignIsActive)
                {
                    kampValue = "-".PadRight(colCampaign);
                    kampType = "-".PadRight(colType);
                }
                else if (p.CampaignPrice > 0)
                {
                    kampValue = $"{p.CampaignPrice:0.00}".PadRight(colCampaign);
                    kampType = "Fast pris".PadRight(colType);
                }
                else if (p.CampaignPercent > 0)
                {
                    decimal newPrice = p.Price * (1 - (p.CampaignPercent / 100m));
                    kampValue = $"{newPrice:0.00}".PadRight(colCampaign);
                    kampType = "Rabatt %".PadRight(colType);
                }
                else if (p.MultiBuyQuantity > 0)
                {
                    kampValue = $"{p.MultiBuyQuantity} för {p.MultiBuyPrice:0.00}".PadRight(colCampaign);
                    kampType = "Multiköp".PadRight(colType);
                }
                else
                {
                    kampValue = "-".PadRight(colCampaign);
                    kampType = "-".PadRight(colType);
                }

                Console.WriteLine(
                    p.PLU.PadRight(colPLU) + " | " +
                    p.Name.PadRight(colName) + " | " +
                    unit.PadRight(colUnit) + " | " +
                    ordPrice + " | " +
                    kampValue + " | " +
                    kampType
                );
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Tryck valfri tangent för att återgå..");
            Console.ResetColor();
            Console.ReadKey();
        }

        /// <summary>
        /// Compact three-column cashier list with campaign rows.
        /// Used only in cashier mode.
        /// </summary>
        public static void ShowCompactPLU(List<Product> products)
        {
            var sorted = products.OrderBy(p => p.Name).ToList();

            int colWidth = 23;          // 23 * 3 = 69 (nära 70)
            int itemsPerRow = 3;
            int rowCount = (int)Math.Ceiling(sorted.Count / (double)itemsPerRow);

            for (int i = 0; i < rowCount; i++)
            {
                for (int col = 0; col < itemsPerRow; col++)
                {
                    int index = i + col * rowCount;
                    if (index >= sorted.Count)
                    {
                        Console.Write("".PadRight(colWidth));
                        continue;
                    }

                    var p = sorted[index];

                    string plu = p.PLU.PadRight(5);
                    string name = (p.Name ?? "").PadRight(colWidth - 6);

                    Console.Write($"{plu} {name}");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

    }
}