// using-raderna gör att AdminController kan använda:
// - Core-lagret: modeller som Product och PriceUnit
// - Data-lagret: filhantering som ProductFileLoader
using CashRegisterSystem.Core;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.Controllers
{
    public static class AdminController
    {
        public static void ShowAdminMenu(List<Product> products)
        {
            // Håller reda på vilket menyval som är markerat
            int adminMenuOption = 1;

            // Styr när användaren tryckt Enter
            bool isSelected = false;

            // Läser tangenttryckningar utan att visa dem
            ConsoleKeyInfo key;

            // Dölj markören för snyggare meny
            Console.CursorVisible = false;

            // Grön färg för markerat val
            string highlight = "\u001b[32m";

            // Menyloop – visas tills användaren trycker Enter
            while (!isSelected)
            {
                Console.Clear();
                Console.WriteLine("=== ADMIN MENY ===");

                // Skriver ut menyalternativen och markerar det valda
                Console.WriteLine($"{(adminMenuOption == 1 ? highlight : "")}> Ändra produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 2 ? highlight : "")}> Lägg till produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 3 ? highlight : "")}> Ta bort produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 4 ? highlight : "")}> Tillbaka\u001b[0m");

                // Läs tangent
                key = Console.ReadKey(true);

                // Hantera piltangenter och Enter
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        adminMenuOption = adminMenuOption == 4 ? 1 : adminMenuOption + 1;
                        break;

                    case ConsoleKey.UpArrow:
                        adminMenuOption = adminMenuOption == 1 ? 4 : adminMenuOption - 1;
                        break;

                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            // Visa markören igen
            Console.CursorVisible = true;

            // Kör valt alternativ
            switch (adminMenuOption)
            {
                case 1:
                    AdminController.EditProduct(products);
                    break;

                case 2:
                    AdminController.AddProduct(products);
                    break;

                case 3:
                    AdminController.DeleteProduct(products);
                    break;

                case 4:
                    return;
            }
        }

        public static void EditProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== ÄNDRA PRODUKT ===\n");

            // Laddar produkter från fil (säkerställer att listan är uppdaterad)
            var loadedProducts = ProductFileLoader.GetProducts();

            // Visar alla produkter
            foreach (var p in products)
            {
                Console.WriteLine($"{p.PLU} - {p.Name} - {p.Price} kr");
            }

            Console.Write("\nAnge PLU: ");
            string? plu = Console.ReadLine();

            // Validerar att PLU inte är tomt
            if (string.IsNullOrWhiteSpace(plu))
            {
                Console.WriteLine("PLU får inte vara tomt.");
                return;
            }

            // Hittar produkten baserat på PLU
            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                Console.WriteLine("Ingen produkt hittades med det PLU-numret.");
                return;
            }

            // Visar nuvarande värden
            Console.WriteLine($"\nVald produkt:");
            Console.WriteLine($"1. Namn: {product.Name}");
            Console.WriteLine($"2. Pris: {product.Price}");
            Console.WriteLine($"3. Enhet: {product.PriceUnit}");
            Console.WriteLine("\nTryck Enter för att behålla nuvarande värde.");

            // Ändra namn
            Console.Write("Ändra namn: ");
            string? newNameProduct = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newNameProduct))
            {
                product.Name = newNameProduct;
            }

            // Ändra pris
            Console.Write("Ändra pris: ");
            string? newPriceProduct = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newPriceProduct) &&
                decimal.TryParse(newPriceProduct, out decimal newPrice))
            {
                product.Price = newPrice;
            }

            // Ändra enhet
            Console.WriteLine("Välj ny enhet:");
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val (Enter för att behålla): ");

            string? unitChoice = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(unitChoice))
            {
                switch (unitChoice)
                {
                    case "1":
                        product.PriceUnit = PriceUnit.PerKilo;
                        break;

                    case "2":
                        product.PriceUnit = PriceUnit.Each;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val. Enheten ändrades inte.");
                        break;
                }
            }

            // Sparar ändringarna till fil
            ProductFileLoader.SaveProducts(products);

            Console.WriteLine($"\nProdukten har uppdaterats!");
        }

        public static void AddProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== LÄGG TILL PRODUKT ===\n");

            // (Här kommer du lägga till logik senare)
        }

        public static void DeleteProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== TA BORT PRODUKT ===\n");

            // (Här kommer du lägga till logik senare)
        }
    }
}
