using CashRegisterSystem.Models;
using CashRegisterSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CashRegisterSystem.Data.FileLoader
{
    /// <summary>
    /// Loads and saves products from a text file.
    /// Creates default products if the file does not exist.
    /// Ensures safe parsing and prevents crashes.
    /// </summary>
    public static class ProductFileLoader
    {
        private static readonly string filePath =
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Products", "products.txt");

        // Default products used when file does not exist.
        private static readonly List<Product> DefaultProducts = new()
        {
            new Product { PLU = "3174", Name = "Apelsin", Price = 19, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "3271", Name = "Avokado", Price = 18, PriceUnit = PriceUnit.Each },
            new Product { PLU = "4011", Name = "Bananer", Price = 29, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4666", Name = "Lök gul", Price = 9, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4182", Name = "Lök röd", Price = 12, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "3131", Name = "Kiwi", Price = 9, PriceUnit = PriceUnit.Each },
            new Product { PLU = "4252", Name = "Jordgubbar", Price = 35, PriceUnit = PriceUnit.Each },
            new Product { PLU = "3220", Name = "Päron", Price = 28, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4686", Name = "Paprika grön", Price = 41, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4687", Name = "Paprika gul", Price = 46, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4688", Name = "Paprika röd", Price = 42, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4833", Name = "Potatis fast", Price = 12, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4863", Name = "Potatis mjölig", Price = 12, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "4521", Name = "Sparris", Price = 46, PriceUnit = PriceUnit.Each },
            new Product { PLU = "4253", Name = "Sallad isberg", Price = 24, PriceUnit = PriceUnit.Each },
            new Product { PLU = "4800", Name = "Selleri", Price = 16, PriceUnit = PriceUnit.Each },
            new Product { PLU = "4645", Name = "Svamp champinjon", Price = 19, PriceUnit = PriceUnit.Each },
            new Product { PLU = "4806", Name = "Tomat kvist", Price = 26, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "3214", Name = "Äpple", Price = 32, PriceUnit = PriceUnit.PerKilo },
            new Product { PLU = "3264", Name = "Vitlök", Price = 16, PriceUnit = PriceUnit.Each },
            new Product { PLU = "3211", Name = "Vitkål", Price = 19, PriceUnit = PriceUnit.Each },
        };

        // Loads all products from file or creates defaults.
        public static List<Product> GetProducts()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            if (!File.Exists(filePath))
            {
                SaveProducts(DefaultProducts);
                return DefaultProducts;
            }

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

            // Apply campaign price to all products.
            foreach (var p in products)
            {
                p.CampaignPrice = CampaignService.GetPrice(p);
            }

            return products;
        }

        // Saves all products to file.
        public static void SaveProducts(List<Product> products)
        {
            if (products == null)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            var lines = products.Select(p =>
                $"{p.PLU};{p.Name};{p.Price};{p.PriceUnit}");

            File.WriteAllLines(filePath, lines);
        }
    }
}
