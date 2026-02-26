using CashRegisterSystem.Data;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    public class ViewAllCampaigns
    {
        public static void ViewCampaigns()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("STARTA KAMPANJER");

            var campaigns = CampaignRepository.GetCampaigns();

            if (!campaigns.Any())
            {
                Console.WriteLine("Inga kampanjer finns.");
                Console.ReadKey();
                return;
            }

            var products = ProductFileLoader.GetProducts();

            foreach (var c in campaigns)
            {
                // Hitta produkten som kampanjen gäller och räkna ut kampanjpriset.
                // product?.Price = ta priset om product inte är null, annars null.
                // ?? 0 = om värdet är null, använd 0 istället.
                var product = products.FirstOrDefault(p => p.Name == c.Name);
                decimal originalPrice = product?.Price ?? 0;
                decimal discountedPrice = originalPrice - (originalPrice * (c.Discount / 100m));

                string status =
                    c.EndDate < DateTime.Today ? "AVSLUTAD" :
                    c.StartDate > DateTime.Today ? "KOMMANDE" : "AKTIV";

                Console.WriteLine($"{c.Id}. {c.Name} - {c.Discount}% - {status} " +
                    $"({c.StartDate:yyyy-MM-dd} → {c.EndDate:yyyy-MM-dd})" +
                    $"| Ord pris: {originalPrice} kr | Kampanjpris: {discountedPrice:F2} kr");
            }
            Console.WriteLine("Tryck ENTER för att återgå till menyn ..");
            Console.ReadKey(true);

        }
    }
}
