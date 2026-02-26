using CashRegisterSystem.Data;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    /// <summary>
    /// Displays all campaigns and shows their status, pricing, and active dates.
    /// </summary>
    public class ViewAllCampaigns
    {
        /// <summary>
        /// Retrieves all campaigns, calculates their discounted prices,
        /// determines their current status, and prints them to the console.
        /// </summary>
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
