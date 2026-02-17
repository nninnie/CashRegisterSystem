using CashRegisterSystem.Core;

namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Provides file-based loading and saving of product data for the cash register system.
    /// </summary>
    /// <remarks>
    /// If the product file does not exist, a predefined set of default products is returned.
    /// Products are stored as semicolon-delimited lines in a text file.
    /// </remarks>
    public static class ProductFileLoader
    {
        /// <summary>
        /// The file path where all product data is stored.
        /// </summary>
        private static readonly string filePath = "products.txt";

        /// <summary>
        /// A predefined list of products used when the product file does not exist.
        /// Ensures the system always has initial product data.
        /// </summary>
        private static readonly List<Product> DefaultProducts = new()
        {
            new Product { PLU = "4011", Name = "Bananer", Price = 29.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "3214", Name = "Äpple", Price = 32.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "3271", Name = "Avokado", Price = 18.90m, PriceUnit = PriceUnit.Each },
            new Product { PLU = "3131", Name = "Kiwi", Price = 9.90m, PriceUnit = PriceUnit.Each },
            new Product { PLU = "3174", Name = "Apelsin", Price = 19.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "3220", Name = "Päron", Price = 28.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4252", Name = "Jordgubbar", Price = 35.09m, PriceUnit = PriceUnit.Each }, // FIX: unik PLU
            new Product { PLU = "4521", Name = "Sparris", Price = 46.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4666", Name = "Lök gul", Price = 9.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4182", Name = "Lök röd", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4688", Name = "Paprika röd", Price = 42.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4687", Name = "Paprika gul", Price = 46.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4686", Name = "Paprika grön", Price = 41.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4833", Name = "Potatis fast", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4863", Name = "Potatis mjölig", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4806", Name = "Tomat kvist", Price = 26.90m, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4253", Name = "Sallad isberg", Price = 24.40m, PriceUnit = PriceUnit.Each }, // FIX: unik PLU
            new Product { PLU = "4645", Name = "Svamp champinjon", Price = 19.90m, PriceUnit = PriceUnit.Each },
            new Product { PLU = "3264", Name = "Vitlök", Price = 16.90m, PriceUnit = PriceUnit.Each },
        };

        /// <summary>
        /// Loads all products from the product file. If the file does not exist,
        /// the predefined default products are returned.
        /// </summary>
        /// <returns>A list of products loaded from file or default products.</returns>
        public static List<Product> GetProducts()
        {
            if (!File.Exists(filePath))
                return DefaultProducts;

            var products = new List<Product>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(';');
                if (parts.Length != 4)
                    continue;

                if (!decimal.TryParse(parts[2], out decimal price))
                    continue;

                if (!Enum.TryParse(parts[3], out PriceUnit unit))
                    continue;

                products.Add(new Product
                {
                    PLU = parts[0],
                    Name = parts[1],
                    Price = price,
                    PriceUnit = unit
                });
            }

            return products;
        }

        /// <summary>
        /// Saves all products to the product file using semicolon-delimited formatting.
        /// </summary>
        /// <param name="products">The list of products to save.</param>
        public static void SaveProducts(List<Product> products)
        {
            var lines = products.Select(p =>
                $"{p.PLU};{p.Name};{p.Price};{p.PriceUnit}");

            File.WriteAllLines(filePath, lines);
        }
    }
}
