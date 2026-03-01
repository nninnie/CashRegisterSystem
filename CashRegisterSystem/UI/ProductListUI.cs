using CashRegisterSystem.Models;

namespace CashRegisterSystem.UI
{
    public static class ProductListUI
    {
        public static void ShowProductList(List<Product> products)
        {
            Console.Clear();
            BannerUI.PrintHeader("PLU-LISTA");
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------");

            // Sort alphabetically by product name
            var sortedProducts = products.OrderBy(product => product.Name).ToList();

            foreach (var product in sortedProducts)
            {
                PrintProduct(product);
            }

            Console.WriteLine("-----------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nÅtergå till menyn");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        private static void PrintProduct(Product product)
        {
            string unitLabel = product.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

            bool hasActiveCampaign =
                product.CampaignPrice > 0 &&
                product.CampaignPrice < product.Price;

            decimal displayPrice = hasActiveCampaign
                ? product.CampaignPrice
                : product.Price;

            string pluColumn = product.PLU.PadRight(8);
            string nameColumn = product.Name.PadRight(25);
            string priceColumn = $"{displayPrice:0.00} {unitLabel}".PadRight(12);

            string originalPriceColumn = hasActiveCampaign
                ? $"(Ord: {product.Price:0.00})"
                : "";

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(pluColumn);
            Console.Write(nameColumn);

            Console.ForegroundColor = hasActiveCampaign
                ? ConsoleColor.Green
                : ConsoleColor.White;

            Console.Write(priceColumn);

            if (hasActiveCampaign)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(originalPriceColumn);
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}