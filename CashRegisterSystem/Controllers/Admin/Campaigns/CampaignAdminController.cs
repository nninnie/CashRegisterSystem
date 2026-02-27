using CashRegisterSystem.Data;
using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    /// <summary>
    /// Handles all admin actions related to campaigns:
    /// viewing, creating, and ending campaigns.
    /// </summary>
    public static class CampaignAdminController
    {
        public static void ViewAllCampaigns()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("VISA KAMPANJER");

            var campaigns = CampaignFileLoader.GetCampaigns();

            if (!campaigns.Any())
            {
                ProductInputUI.ShowError("Inga kampanjer finns.");
                return;
            }

            var products = ProductFileLoader.GetProducts();

            Console.WriteLine("-----------------------------------------");

            foreach (var c in campaigns)
            {
                var product = products.FirstOrDefault(p => p.Name == c.Name);
                decimal originalPrice = product?.Price ?? 0;
                decimal discountedPrice = originalPrice - (originalPrice * (c.Discount / 100m));

                string status =
                    c.EndDate < DateTime.Today ? "AVSLUTAD" :
                    c.StartDate > DateTime.Today ? "KOMMANDE" : "AKTIV";

                Console.WriteLine(
                    $"{c.Id}. {c.Name} - {c.Discount}% - {status} " +
                    $"({c.StartDate:yyyy-MM-dd} → {c.EndDate:yyyy-MM-dd}) | " +
                    $"Ord pris: {originalPrice:0.00} kr | Kampanjpris: {discountedPrice:0.00} kr"
                );
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("\nTryck valfri tangent för att återgå...");
            Console.ReadKey(true);
        }


        public static void StartCampaign()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("STARTA KAMPANJ");

            var products = ProductFileLoader.GetProducts();

            if (!products.Any())
            {
                ProductInputUI.ShowError("Produktlistan är tom.");
                return;
            }

            ProductListUI.ShowCompactProductListAdmin(products);

            Console.Write("\nPLU: ");
            string plu = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(plu))
            {
                ProductInputUI.ShowError("PLU får inte vara tomt.");
                return;
            }

            var selectedProduct = products.FirstOrDefault(p => p.PLU == plu);

            if (selectedProduct == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            Console.Write("Rabatt %: ");
            if (!decimal.TryParse(Console.ReadLine(), out var discount) || discount < 0)
            {
                ProductInputUI.ShowError("Ogiltig rabatt.");
                return;
            }

            Console.Write("Startdatum (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                ProductInputUI.ShowError("Ogiltigt startdatum.");
                return;
            }

            Console.Write("Slutdatum (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate) || endDate < startDate)
            {
                ProductInputUI.ShowError("Ogiltigt slutdatum.");
                return;
            }

            var campaigns = CampaignFileLoader.GetCampaigns();
            int newId = campaigns.Any() ? campaigns.Max(c => c.Id) + 1 : 1;

            var newCampaign = new Campaign
            {
                Id = newId,
                Name = selectedProduct.Name,
                Discount = discount,
                StartDate = startDate,
                EndDate = endDate,
                IsActive = true
            };

            campaigns.Add(newCampaign);
            CampaignFileLoader.SaveCampaigns(campaigns);

            ProductInputUI.ShowConfirmation(
                "Kampanj skapad!",
                $"Produkt: {selectedProduct.Name}\n" +
                $"Rabatt: {discount}%\n" +
                $"Period: {startDate:yyyy-MM-dd} → {endDate:yyyy-MM-dd}"
            );
        }


        public static void EndCampaign()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("AVSLUTA KAMPANJ");

            var campaigns = CampaignFileLoader.GetCampaigns();
            if (!campaigns.Any())
            {
                ProductInputUI.ShowError("Inga kampanjer finns.");
                return;
            }

            var products = ProductFileLoader.GetProducts();
            if (!products.Any())
            {
                ProductInputUI.ShowError("Produktlistan är tom.");
                return;
            }

            ProductListUI.ShowCompactProductListAdmin(products);

            Console.Write("\nPLU: ");
            string plu = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(plu))
            {
                ProductInputUI.ShowError("PLU får inte vara tomt.");
                return;
            }

            var product = products.FirstOrDefault(p => p.PLU == plu);
            if (product == null)
            {
                ProductInputUI.ShowError("Ingen produkt hittades.");
                return;
            }

            var campaign = campaigns.FirstOrDefault(c => c.Name == product.Name && c.IsActive);
            if (campaign == null)
            {
                ProductInputUI.ShowError("Ingen aktiv kampanj för denna produkt.");
                return;
            }

            campaign.IsActive = false;
            CampaignFileLoader.SaveCampaigns(campaigns);

            ProductInputUI.ShowConfirmation(
                "Kampanj avslutad!",
                $"Produkt: {product.Name}\nRabatt: {campaign.Discount}%"
            );
        }
    }
}
