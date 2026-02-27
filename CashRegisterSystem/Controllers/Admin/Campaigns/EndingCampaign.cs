using CashRegisterSystem.Controllers.Admin.Products;
using CashRegisterSystem.Data;
using CashRegisterSystem.Data.FileLoader;
using CashRegisterSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Controllers.Admin.Campaigns
{
    /// <summary>
    /// Handles the process of ending active campaigns in the admin section.
    /// </summary>
    public static class EndingCampaign
    {
        /// <summary>
        /// Ends a selected campaign by matching the entered PLU with a product,
        /// validating that a campaign exists for it, and saving the updated status.
        /// </summary>
        public static void EndCampaign()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("AVSLUTA KAMPANJER");

            var campaign = CampaignFileLoader.GetCampaigns();

            if (!campaign.Any())
            {
                Console.WriteLine("Inga kampanjer finns.");
                Console.ReadKey();
                return;
            }

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

            ProductUI.ShowProducts(productList);

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

            if (selectedCampaign == null)
            {
                Console.WriteLine("Ingen kampanj hittades för denna produkt");
                Console.WriteLine("\nTryck ENTER för att gå tillbaka till menyn..");
                Console.ReadKey(true);
                return;
            }

            selectedCampaign.IsActive = false;

            CampaignFileLoader.SaveCampaigns(campaign); 

            Console.WriteLine("\nKampanjen har avslutats!");
            Console.ReadKey();
        }
    }
}
