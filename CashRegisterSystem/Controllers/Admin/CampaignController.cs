using CashRegisterSystem.Core;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.Controllers.Admin
{
    /// <summary>
    /// Handles user interactions and operations related to campaigns in the cash register system.
    /// </summary>
    /// <remarks>
    /// This controller provides functionality for viewing, creating, editing, and managing campaigns. 
    /// It communicates with <see cref="CampaignRepository"/> for data persistence and ensures that 
    /// campaign-related actions are performed consistently and validated before being saved.
    /// </remarks>
    public class CampaignController
    {
        /// <summary>
        /// Displays the campaign menu in the console and handles user interaction for campaign-related actions.
        /// </summary>
        /// <remarks>This method presents a menu that allows the user to create, view, or end campaigns,
        /// or return to the previous menu. The method blocks until the user makes a selection and then invokes the
        /// corresponding action. The console cursor visibility is managed during menu navigation for a better user
        /// experience.</remarks>
        public static void ShowCampaignMenu()
        {
            int menuOption = 1; // Startar menyn på första alternativet
            bool isSelected = false; // Blir true när användaren trycker Enter och gör sitt val
            ConsoleKeyInfo key; // Lagrar vilken tangent som trycks ned
            Console.CursorVisible = false; // Dölj markören för en renare meny
            string highlight = "\u001b[32m"; // grön markering

            while (!isSelected)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("            KAMPANJ-MENY");
                Console.ResetColor();
                Console.WriteLine("========================================");
                Console.WriteLine();

                Console.WriteLine($"{(menuOption == 1 ? "·" + highlight : "")} Skapa kampanj\u001b[0m");
                Console.WriteLine($"{(menuOption == 2 ? "·" + highlight : "")} Visa kampanjer\u001b[0m");
                Console.WriteLine($"{(menuOption == 3 ? "·" + highlight : "")} Avsluta kampanj\u001b[0m");
                Console.WriteLine($"{(menuOption == 4 ? "·" + highlight : "")} Tillbaka\u001b[0m");

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        menuOption = menuOption == 4 ? 1 : menuOption + 1;
                        break;
                    case ConsoleKey.UpArrow:
                        menuOption = menuOption == 1 ? 4 : menuOption - 1;
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            Console.CursorVisible = true;

            switch (menuOption)
            {
                case 1:
                    StartCampaignMenu();
                    break;
                case 2:
                    ViewCampaigns();
                    break;
                case 3:
                    EndCampaigns();
                    break;
                case 4:
                    return;
            }

        }

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

            Console.WriteLine("========================================");
            Console.WriteLine("             STARTA KAMPANJ");
            Console.WriteLine("========================================");
            Console.WriteLine();

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
        /// <summary>
        /// Displays a list of all available campaigns and their details in the console.
        /// </summary>
        /// <remarks>The method clears the console and presents each campaign with its name, discount,
        /// status, date range, original price, and discounted price. If no campaigns are available, a message is shown
        /// to the user. The method waits for user input before returning to the menu.</remarks>
        public static void ViewCampaigns()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("           VISA KAMPANJER");
            Console.WriteLine("========================================\n");
            Console.WriteLine();

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

        /// <summary>
        /// Ends an active campaign associated with a product by deactivating it and saving the updated campaign list.
        /// </summary>
        /// <remarks>This method prompts the user to enter a product PLU, identifies the corresponding
        /// campaign, and marks it as inactive. If no campaigns or products are available, or if the specified PLU does
        /// not match any product or campaign, the method displays an appropriate message and returns. The method
        /// interacts with the console for user input and feedback, and updates persistent campaign data via the
        /// repository.</remarks>
        public static void EndCampaigns()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("          AVSLUTA KAMPANJER");
            Console.WriteLine("========================================\n");
            Console.WriteLine();

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
