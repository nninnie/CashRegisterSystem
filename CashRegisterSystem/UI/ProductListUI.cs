using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class ProductListUI
    {
        public static void ShowProductList(List<Product> products)
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("PLU-LISTA");
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------");

            // Sortera alfabetiskt efter namn
            products = products.OrderBy(p => p.Name).ToList();

            foreach (var p in products)
            {
                PrintProduct(p);
            }

            Console.WriteLine("-----------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nÅtergå till menyn");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        private static void PrintProduct(Product p)
        {
            string unit = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

            bool hasCampaign = p.CampaignPrice > 0 && p.CampaignPrice < p.Price;

            // Kolumn 1
            string col1 = $"{p.PLU}   {p.Name}".PadRight(22);

            // Kolumn 2
            decimal displayPrice = hasCampaign ? p.CampaignPrice : p.Price;
            string col3 = $"{displayPrice:0.00} {unit}".PadRight(18);

            // Kolumn 3
            string col2 = hasCampaign ? $"(Ord: {p.Price:0.00})" : "";

            // Utskrift
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(col1);

            Console.ForegroundColor = hasCampaign ? ConsoleColor.Green : ConsoleColor.White;
            Console.Write(col3);

            if (hasCampaign)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(col2);
            }

            Console.ResetColor();
            Console.WriteLine();
        }

    }
}
