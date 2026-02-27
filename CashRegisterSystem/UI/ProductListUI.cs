using CashRegisterSystem.Models;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.UI
{
    public static class ProductListUI
    {
        public static void ShowProductList(List<Product> products)
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("PRODUKTLISTA");

            var campaigns = CampaignFileLoader.GetCampaigns();

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($" {"PLU",-6} {"Produkt",-20} {"Pris"}");
            Console.WriteLine("-----------------------------------------");

            foreach (var p in products.OrderBy(p => p.Name))
            {
                string unit = p.PriceUnit == PriceUnit.PerKilo ? "kr/kg" : "kr/st";

                var activeCampaign = campaigns
                    .FirstOrDefault(c => c.Name == p.Name && c.IsActive);

                decimal priceToShow = ProductInputUI.GetPrice(p);

                if (activeCampaign != null)
                {
                    decimal campaignPrice = ProductInputUI.GetPrice(p);

                    // Rad 1 – kampanjpris i mörkgrönt
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(
                        $" {p.PLU,-6} {p.Name,-20} {campaignPrice:0.00} {unit}"
                    );
                    Console.ResetColor();

                    // Rad 2 – ordinarie pris i grått, perfekt indraget
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(
                        $" {"",-6} {"",-20} (Ord: {p.Price:0.00} {unit})"
                    );
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(
                        $" {p.PLU,-6} {p.Name,-20} {p.Price:0.00} {unit}"
                    );
                }
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("\nTryck valfri tangent för att återgå till menyn...");
            Console.ReadKey();
        }
    }
}
