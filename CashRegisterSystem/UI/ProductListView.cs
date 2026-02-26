using CashRegisterSystem.Models;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.UI
{
    public static class ProductListView
    {
        public static void ShowProductList(List<Product> products)
        {
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

                decimal priceToShow = ProductUI.GetPrice(p);

                if (activeCampaign != null)
                {
                    // Kampanjpris i grönt
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(
                        $" {p.PLU,-6} {p.Name,-20} {priceToShow:0.00} {unit} (Kampanj!)"
                    );
                    Console.ResetColor();

                    // Ordinarie pris i grått
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(
                        $" {"",-6} {"",-20} Ordinarie: {p.Price:0.00} {unit}"
                    );
                    Console.ResetColor();
                }
                else
                {
                    // Vanligt pris
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