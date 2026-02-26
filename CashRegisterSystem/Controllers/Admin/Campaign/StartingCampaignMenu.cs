using CashRegisterSystem.Data;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers.Admin.Campaign
{
    public class StartingCampaignMenu
    {
        /// <summary>
        /// Displays an interactive menu for creating a new campaign for a selected product. Prompts the user for
        /// product selection, discount details, and campaign dates, then saves the new campaign if all inputs are
        /// valid.
        /// </summary>
        /// <remarks>This method is intended for use in a console application and requires user input via
        /// the console. The method validates all user inputs and provides feedback for invalid entries. Existing
        /// products and campaigns are loaded from persistent storage, and the new campaign is saved upon successful
        /// creation.</remarks>
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

            ProductController.PrintProducts(productList);

            Console.WriteLine("Skriv in PLU för varan du vill skapa kampanj på: ");
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

            Console.Write("Hur mycket rabatt ska varan ha? ");
            string discountInput = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(discountInput))
            {
                Console.WriteLine("Du måste ange rabatt: ");
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

            Console.WriteLine("Ange startdatum: ");
            string startInput = Console.ReadLine()!;

            if (!DateTime.TryParse(startInput, out DateTime startDate))
            {
                Console.WriteLine("Ogiltigt startdatum.");
                return;
            }

            Console.WriteLine("Ange slutdatum: ");
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
