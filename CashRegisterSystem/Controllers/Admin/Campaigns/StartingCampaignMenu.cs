using CashRegisterSystem.Core;
using CashRegisterSystem.Data;
using CashRegisterSystem.UI;
using CashRegisterSystem.Controllers.Admin.Campaigns;
using CashRegisterSystem.Controllers.Admin.Products;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    /// <summary>
    /// Provides the admin workflow for creating new campaigns for selected products.
    /// </summary>
    public class StartingCampaignMenu
    {
        /// <summary>
        /// Guides the admin through selecting a product, entering discount details,
        /// setting campaign dates, and saving the new campaign.
        /// </summary>
        public static void StartCampaignMenu()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("STARTA KAMPANJER");

            // Ladda produkter och spara i listan products
            var productList = ProductFileLoader.GetProducts();

            if (!productList.Any())
            {
                Console.Write("Listan är tom.");
                return;
            }

            ProductUI.ShowProducts(productList);

            Console.Write("\nPLU: ");
            string campaignPlu = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(campaignPlu))
            {
                Console.WriteLine("PLU får inte vara tom. Försök igen!");
                return;
            }

            // hämtar en selectedProduct som söker igenom productList och hämtar produkten som matchar det angivna PLU-numret
            var selectedProduct = productList.FirstOrDefault(p => p.PLU == campaignPlu);

            if (selectedProduct == null)
            {
                Console.WriteLine("Ingen produkt hittades!");
                return;
            }

            Console.Write("Rabatt %: ");
            string discountInput = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(discountInput))
            {
                Console.WriteLine("Du måste ange ett tal %: ");
                return;
            }

            if (!decimal.TryParse(discountInput, out var discountPercent))
            {
                Console.WriteLine("Rabatten måste vara ett tal!");
                return;
            }
            if (discountPercent < 0)
            {
                Console.Write("Rabatten kan inte vara negativ");
                return;
            }

            Console.WriteLine("Ange startdatum (yyyy-MM-dd): ");
            string startInput = Console.ReadLine()!;

            if (!DateTime.TryParse(startInput, out DateTime startDate))
            {
                Console.WriteLine("Ogiltigt startdatum.");
                return;
            }

            Console.WriteLine("Ange slutdatum (yyyy-MM-dd): ");
            string endInput = Console.ReadLine()!;

            if (!DateTime.TryParse(endInput, out DateTime endDate))
            {
                Console.WriteLine("Ogiltigt slutdatum.");
                return;
            }

            if (endDate < startDate)
            {
                Console.WriteLine("Slutdatum kan inte vara före startdatum.");
                return;
            }

            // Hämtar alla befintliga kampanjer, skapar ett nytt unikt ID baserat på högsta befintliga ID
            // bygger ett nytt Campaign-objekt med användarens värden, lägger till det i listan och sparar tillbaka till fil
            var campaignList = CampaignRepository.GetCampaigns();

            int newID = campaignList.Any() ? campaignList.Max(c => c.Id) + 1 : 1;

            var newCampaign = new Campaign
            {
                Id = newID,
                Name = selectedProduct.Name,
                Discount = discountPercent,
                StartDate = startDate,
                EndDate = endDate,
                IsActive = true
            };

            campaignList.Add(newCampaign);

            CampaignRepository.SaveCampaigns(campaignList);

            Console.WriteLine("\nKampanj skapad!");
            Console.WriteLine($"Produkt: {selectedProduct.Name}");
            Console.WriteLine($"Rabatt: {discountPercent}%");
            Console.WriteLine($"Gäller från {startDate:yyyy-MM-dd} till {endDate:yyyy-MM-dd}");
        }
    }
}
