using CashRegisterSystem.Core;
using CashRegisterSystem.Data;

namespace CashRegisterSystem.Controllers.Admin
{
    public static class AdminController
    {
        public static void ShowAdminMenu(List<Product> products)
        {
            int adminMenuOption = 1;
            bool isSelected = false;
            ConsoleKeyInfo key;
            Console.CursorVisible = false;
            string highlight = "\u001b[32m";

            while (!isSelected)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("                ADMINMENY");
                Console.ResetColor();
                Console.WriteLine("========================================");
                Console.WriteLine();


                Console.WriteLine($"{(adminMenuOption == 1 ? "·" + highlight : "")} Ändra produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 2 ? "·" + highlight : "")} Lägg till produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 3 ? "·" + highlight : "")} Ta bort produkt\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 4 ? "·" + highlight : "")} Starta kampanj\u001b[0m");
                Console.WriteLine($"{(adminMenuOption == 5 ? "·" + highlight : "")} Tillbaka till menyn\u001b[0m");

                Console.WriteLine();

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        adminMenuOption = adminMenuOption == 5 ? 1 : adminMenuOption + 1;
                        break;

                    case ConsoleKey.UpArrow:
                        adminMenuOption = adminMenuOption == 1 ? 5 : adminMenuOption - 1;
                        break;

                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            Console.CursorVisible = true;

            switch (adminMenuOption)
            {
                case 1:
                    EditProduct(products);
                    break;

                case 2:
                    AddProduct(products);
                    break;

                case 3:
                    DeleteProduct(products);
                    break;

                case 4:
                    //StartCampaign(products);
                    break;

                case 5:
                    return;
            }
        }

        // ============================
        // ÄNDRA PRODUKT
        // ============================
        public static void EditProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             ÄNDRA PRODUKT");
            Console.WriteLine("========================================");
            Console.WriteLine();


            var loadedProducts = ProductFileLoader.GetProducts();

            foreach (var p in loadedProducts)
                Console.WriteLine($"{p.PLU} - {p.Name} - {p.Price} kr");

            Console.Write("\nAnge PLU: ");
            string? plu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(plu))
            {
                Console.WriteLine("PLU får inte vara tomt.");
                Console.ReadKey();
                return;
            }

            var product = loadedProducts.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                Console.WriteLine("Ingen produkt hittades.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nVald produkt:");
            Console.WriteLine($"1. Namn: {product.Name}");
            Console.WriteLine($"2. Pris: {product.Price}");
            Console.WriteLine($"3. Enhet: {product.PriceUnit}");
            Console.WriteLine("\nTryck Enter för att behålla nuvarande värde.");

            Console.Write("Ändra namn: ");
            string? newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                product.Name = newName;

            Console.Write("Ändra pris: ");
            string? newPriceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPriceInput) &&
                decimal.TryParse(newPriceInput, out decimal newPrice))
                product.Price = newPrice;

            Console.WriteLine("Välj ny enhet:");
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val: ");
            string? unitChoice = Console.ReadLine();

            if (unitChoice == "1") product.PriceUnit = PriceUnit.PerKilo;
            if (unitChoice == "2") product.PriceUnit = PriceUnit.Each;

            ProductFileLoader.SaveProducts(loadedProducts);

            products.Clear();
            products.AddRange(loadedProducts);

            Console.WriteLine("\nProdukten har uppdaterats!");
            Console.ReadKey();
        }

        // ============================
        // LÄGG TILL PRODUKT
        // ============================
        public static void AddProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             LÄGG TILL PRODUKT");
            Console.WriteLine("========================================");
            Console.WriteLine();


            Console.Write("PLU: ");
            string? plu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(plu))
            {
                Console.WriteLine("PLU får inte vara tomt.");
                Console.ReadKey();
                return;
            }

            if (products.Any(p => p.PLU == plu))
            {
                Console.WriteLine("En produkt med detta PLU finns redan.");
                Console.ReadKey();
                return;
            }

            Console.Write("Namn: ");
            string? name = Console.ReadLine();

            Console.Write("Pris: ");
            string? priceInput = Console.ReadLine();

            if (!decimal.TryParse(priceInput, out decimal price))
            {
                Console.WriteLine("Ogiltigt pris.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enhet:");
            Console.WriteLine("1. KG");
            Console.WriteLine("2. STYCK");
            Console.Write("Val: ");
            string? unitChoice = Console.ReadLine();

            PriceUnit unit = unitChoice == "1" ? PriceUnit.PerKilo : PriceUnit.Each;

            var newProduct = new Product
            {
                PLU = plu,
                Name = name!,
                Price = price,
                PriceUnit = unit
            };

            products.Add(newProduct);
            ProductFileLoader.SaveProducts(products);

            Console.WriteLine("\nProdukten har lagts till!");
            Console.ReadKey();
        }

        // ============================
        // TA BORT PRODUKT
        // ============================
        public static void DeleteProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("             TA BORT PRODUKT");
            Console.WriteLine("========================================");
            Console.WriteLine();


            foreach (var p in products)
                Console.WriteLine($"{p.PLU} - {p.Name}");

            Console.Write("\nAnge PLU att ta bort: ");
            string? plu = Console.ReadLine();

            var product = products.FirstOrDefault(p => p.PLU == plu);

            if (product == null)
            {
                Console.WriteLine("Ingen produkt hittades.");
                Console.ReadKey();
                return;
            }

            products.Remove(product);
            ProductFileLoader.SaveProducts(products);

            Console.WriteLine("\nProdukten har tagits bort!");
            Console.ReadKey();
        }

        // ============================
        // STARTA KAMPANJ
        // ============================

        public static void StartCampaignMenu()
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("             STARTA KAMPANJ");
            Console.WriteLine("========================================");
            Console.WriteLine();

            // KOOD
        }


    }
}

