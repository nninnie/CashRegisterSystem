using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides UI helpers for displaying PLU-based product lists.
    /// Contains both a full administrative product list and a compact
    /// cashier-friendly version optimized for fast lookup.
    /// </summary>
    public static class ProductListUI
    {
        /// <summary>
        /// Displays the standard administrative product list in a
        /// table-style layout, showing one row per product with full
        /// pricing and campaign details.
        /// </summary>

        public static void ShowProductList(List<Product> products, bool showReturnMessage = true)
        {
            const int pageSize = 10;
            int currentPage = 0;

            var sorted = products.OrderBy(p => p.Name).ToList();
            int totalPages = (int)Math.Ceiling(sorted.Count / (double)pageSize);

            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();
                BannerUI.PrintHeader("PRODUKTLISTA");

                Console.WriteLine($"Sida {currentPage + 1}/{totalPages}\n");

                int colPLU = 5;
                int colName = 14;
                int colUnit = 5;
                int colPrice = 10;
                int colCampaign = 12;
                int colType = 8;

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(
                    "PLU".PadRight(colPLU) + " | " +
                    "Produkt".PadRight(colName) + " | " +
                    "Enhet".PadRight(colUnit) + " | " +
                    "Ord.pris".PadRight(colPrice) + " | " +
                    "Kampanj".PadRight(colCampaign) + " | " +
                    "Typ".PadRight(colType)
                );
                Console.ResetColor();

                Console.WriteLine(new string('-', 70));

                var pageItems = sorted
                    .Skip(currentPage * pageSize)
                    .Take(pageSize)
                    .ToList();

                foreach (var p in pageItems)
                {
                    string unit = p.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";
                    string ordPrice = $"{p.Price:0.00}".PadRight(colPrice);

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

                    Console.ForegroundColor = p.CampaignIsActive ? ConsoleColor.DarkGray : ConsoleColor.DarkGray;

                    Console.WriteLine(
                        p.PLU.PadRight(colPLU) + " | " +
                        p.Name.PadRight(colName) + " | " +
                        unit.PadRight(colUnit) + " | " +
                        ordPrice + " | " +
                        kampValue + " | " +
                        kampType
                    );

                    Console.ResetColor();
                }

                Console.WriteLine();

                string[] menuItems = { "Nästa sida", "Föregående sida", "Tillbaka" };
                int selected = 0;

                while (true)
                {
                    Console.WriteLine();

                    for (int i = 0; i < menuItems.Length; i++)
                    {
                        if (i == selected)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"> {menuItems[i]}");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"  {menuItems[i]}");
                        }
                    }

                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                        return;

                    if (key == ConsoleKey.UpArrow && selected > 0)
                        selected--;

                    if (key == ConsoleKey.DownArrow && selected < menuItems.Length - 1)
                        selected++;

                    if (key == ConsoleKey.Enter)
                        break;

                    Console.SetCursorPosition(0, Console.CursorTop - menuItems.Length - 1);
                }

                if (selected == 0 && currentPage < totalPages - 1)
                    currentPage++;  

                else if (selected == 1 && currentPage > 0)
                    currentPage--;   

                else if (selected == 2)
                    return;         

            }
        }



        /// <summary>
        /// Compact three-column cashier list with campaign rows.
        /// Used only in cashier mode.
        /// </summary>
        public static void ShowCompactPLU(List<Product> products)
        {
            var sorted = products.OrderBy(p => p.Name).ToList();

            int colWidth = 23;          
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
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{plu} {name}");
                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

    }
}