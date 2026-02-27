using CashRegisterSystem.Models;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.UI
{
    public static class ProductUI
    {
        public static void ShowProducts(List<Product> products)
        {
            var campaigns = CampaignFileLoader.GetCampaigns();

            foreach (var p in products)
            {
                var activeCampaign = campaigns
                    .FirstOrDefault(c => c.Name == p.Name && c.IsActive);

                if (activeCampaign != null)
                {
                    decimal campaignPrice = p.Price * (1 - activeCampaign.Discount / 100);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{p.PLU} - {p.Name} - {campaignPrice:0.00} kr (Kampanj!)");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"   Ordinarie pris: {p.Price:0.00} kr");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{p.PLU} - {p.Name} - {p.Price:0.00} kr");
                }
            }
        }
        public static decimal GetPrice(Product product)
        {
            var campaigns = CampaignFileLoader.GetCampaigns();

            var activeCampaign = campaigns
                .FirstOrDefault(c => c.Name == product.Name && c.IsActive);

            if (activeCampaign == null)
                return product.Price;

            return product.Price * (1 - activeCampaign.Discount / 100);
        }

        public static string AskForPlu()
        {
            Console.Write("Ange PLU: ");
            return Console.ReadLine()!;
        }

        public static string AskForName()
        {
            Console.Write("Namn: ");
            return Console.ReadLine()!;
        }

        public static decimal AskForPrice()
        {
            Console.Write("Pris: ");
            decimal.TryParse(Console.ReadLine(), out decimal price);
            return price;
        }

        public static PriceUnit AskForUnit()
        {
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val: ");
            return Console.ReadLine() == "1" ? PriceUnit.PerKilo : PriceUnit.Each;
        }
    }
}