using CashRegisterSystem.Data;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class PLUDisplayUI
    {
        public static void PrintPLUList(List<Product> products, int maxColumns = 4)
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("KASSA");

            var campaigns = CampaignFileLoader.GetCampaigns();

            int colWidth = 28;
            int columns = Math.Min(maxColumns, (int)Math.Ceiling(products.Count / 8.0));
            int rows = (int)Math.Ceiling(products.Count / (double)columns);

            List<List<Product>> cols = new();
            for (int c = 0; c < columns; c++)
                cols.Add(products.Skip(c * rows).Take(rows).ToList());

            for (int r = 0; r < rows; r++)
            {
                string line = "";

                for (int c = 0; c < columns; c++)
                {
                    if (r < cols[c].Count)
                    {
                        var p = cols[c][r];
                        var activeCampaign = campaigns.FirstOrDefault(x => x.Name == p.Name && x.IsActive);

                        decimal priceToShow = ProductUI.GetPrice(p);
                        string unit = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

                        if (activeCampaign != null)
                        {
                            line += $"{p.PLU} {p.Name} {priceToShow:0.00} {unit}*".PadRight(colWidth);
                        }
                        else
                        {
                            line += $"{p.PLU} {p.Name} {p.Price:0.00} {unit}".PadRight(colWidth);
                        }
                    }
                    else
                    {
                        line += "".PadRight(colWidth);
                    }
                }

                Console.WriteLine(line);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n* Kampanjpris");
            Console.ResetColor();
        }
    }
}