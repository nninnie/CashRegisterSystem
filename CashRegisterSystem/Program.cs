namespace CashRegisterSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // PLU-lista Frukt & Grönt
            List<Product> products = new List<Product>

            {
                new Product { Id = "4061", Name = "Bananer", Price = 29.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "3214", Name = "Äpple", Price = 32.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "3174", Name = "Apelsin", Price = 19.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "3220", Name = "Päron", Price = 28.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "4251", Name = "Jordgubbar", Price = 35.9m, PriceType = PriceType.Each },
                new Product { Id = "4521", Name = "Sparris", Price = 46.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "4666", Name = "Lök gul", Price = 9.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "4688", Name = "Paprika röd", Price = 42.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "4833", Name = "Potatis fast", Price = 12.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "4806", Name = "Tomat kvist", Price = 26.9m, PriceType = PriceType.PerKilo },
                new Product { Id = "4251", Name = "Sallad isberg", Price = 20.0m, PriceType = PriceType.Each },
                new Product { Id = "4645", Name = "Svamp champinjon", Price = 19.9m, PriceType = PriceType.Each },
                new Product { Id = "3264", Name = "Vitlök", Price = 16.9m, PriceType = PriceType.Each },
            };



            while (true)
            {
                //----------MENY------------
                Console.WriteLine("KASSA");
                Console.WriteLine("1. Ny kund");
                Console.WriteLine("2. Visa Produkter");
                Console.WriteLine("3. Admin");
                Console.WriteLine("4. Avsluta programmet");

                Console.Write("\nMenyval: ");

                string input = Console.ReadLine()!;

                switch (input)
                {
                    case "1":
                        break;

                    case "2":
                        Console.WriteLine("\n---Frukt & Grönt---" +
                            "");

                        //loopar igenom produktlistan, skriver ut varje p = produkt
                        foreach (var p in products)
                        {
                            string type = p.PriceType == PriceType.PerKilo ? "kr/kg" : "kr//st";
                            Console.WriteLine($"{p.Id} {p.Name} - {p.Price} {type}");
                        }

                        Console.WriteLine();
                        break;

                    case "3":
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Fel menyval, försök igen.");
                        break;



                }
            }
        }
        // Enum som visar om priset gäller per styck eller per kilo, en egen lista med val som du använder i din kod.
        private enum PriceType
        {
            Each,
            PerKilo
        }


        private class Product
        {
            // Beskriver en produkt i kassan
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public PriceType PriceType { get; set; }

        }
    }
}
