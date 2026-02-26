using CashRegisterSystem.Data;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers.Admin.Campaign
{
    public class EndACampaign
    {

        /// <summary>
        /// Ends an active campaign associated with a product by deactivating it and saving the updated campaign list.
        /// </summary>
        /// <remarks>This method prompts the user to enter a product PLU, identifies the corresponding
        /// campaign, and marks it as inactive. If no campaigns or products are available, or if the specified PLU does
        /// not match any product or campaign, the method displays an appropriate message and returns. The method
        /// interacts with the console for user input and feedback, and updates persistent campaign data via the
        /// repository.</remarks>
        public static void EndCampaign()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("AVSLUTA KAMPANJER");

            var campaign = CampaignRepository.GetCampaigns();

            if (!campaign.Any())
            {
                Console.WriteLine("Inga kampanjer finns.");
                Console.ReadKey();
                return;
            }

            var products = ProductFileLoader.GetProducts();

            Console.WriteLine("Skriv in PLU på vilken kampanj som ska avslutas: ");
            string endCampaign = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(endCampaign))
            {
                Console.WriteLine("PLU får inte vara tom. Försök igen!");
                return;
            }

            // Ladda produkter och spara i listan products
            var productList = ProductFileLoader.GetProducts();

            if (!productList.Any())
            {
                Console.Write("Listan är tom.");
                return;
            }

            ProductController.PrintProducts(productList);


            // hämtar en selectedProduct som söker igenom productList och hämtar produkten som matchar det angivna PLU-numret
            var selectedProduct = productList.FirstOrDefault(p => p.PLU == endCampaign);

            // Kontrollera att produkten finns
            if (selectedProduct == null)
            {
                Console.WriteLine("Ingen produkt hittades!");
                return;
            }

            // Hitta kampanjen som gäller för denna produkt (kampanjer kopplas via produktnamn)
            var selectedCampaign = campaign.FirstOrDefault(c => c.Name == selectedProduct.Name);

            // Kontrollera att kampanjen finns
            if (selectedCampaign == null)
            {
                Console.WriteLine("Ingen kampanj hittades för denna produkt");
                Console.WriteLine("\nTryck ENTER för att gå tillbaka till menyn..");
                Console.ReadKey(true);
                return;
            }

            selectedCampaign.IsActive = false; // Avsluta kampanjen

            CampaignRepository.SaveCampaigns(campaign); // Spara ändringen

            Console.WriteLine("\nKampanjen har avslutats!");
            Console.ReadKey();
        }
    }

}