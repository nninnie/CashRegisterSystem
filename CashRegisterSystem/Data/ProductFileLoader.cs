using CashRegisterSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Data
{
    public static class ProductFileLoader
    {
        // Filen där produkter sparas permanent.
        private static readonly string filePath = "products.txt";


        // En standardlista med produkter som används om filen inte finns. 
        // Detta gör att programmet alltid har produkter att visa första gången det körs.
        private static List<Product> DefaultProducts => new List<Product>
        {
        new Product { PLU = "4011", Name = "Bananer", Price = 29.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "3214", Name = "Äpple", Price = 32.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "3271", Name = "Avokado", Price = 18.90m, PriceUnit = PriceUnit.Each },
                new Product { PLU = "3131", Name = "Kiwi", Price = 9.90m, PriceUnit = PriceUnit.Each },
                new Product { PLU = "3174", Name = "Apelsin", Price = 19.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "3220", Name = "Päron", Price = 28.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4251", Name = "Jordgubbar", Price = 35.09m, PriceUnit = PriceUnit.Each },
                new Product { PLU = "4521", Name = "Sparris", Price = 46.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4666", Name = "Lök gul", Price = 9.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4182", Name = "Lök Röd", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4688", Name = "Paprika röd", Price = 42.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4687", Name = "Paprika gul", Price = 46.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4686", Name = "Paprika grön", Price = 41.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4833", Name = "Potatis fast", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4863", Name = "Potatis mjölig", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4806", Name = "Tomat kvist", Price = 26.90m, PriceUnit = PriceUnit.PerKilo },
                new Product { PLU = "4251", Name = "Sallad isberg", Price = 24.40m, PriceUnit = PriceUnit.Each },
                new Product { PLU = "4645", Name = "Svamp champinjon", Price = 19.90m, PriceUnit = PriceUnit.Each },
                new Product { PLU = "3264", Name = "Vitlök", Price = 16.90m, PriceUnit = PriceUnit.Each },
            };

        // Hämtar produkter från fil om den finns. 
        // Om filen saknas används standardlistan ovan.
        public static List<Product> GetProducts()
        {
            if (!File.Exists(filePath))
                return DefaultProducts;

            var products = new List<Product>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(';');

                products.Add(new Product
                {
                    PLU = parts[0],
                    Name = parts[1],
                    Price = decimal.Parse(parts[2]),
                    PriceUnit = Enum.Parse<PriceUnit>(parts[3])
                });
            }
            return products;
        }

        // Sparar alla produkter till fil filePath = "products.txt"
        public static void SaveProducts(List<Product> products)
        {
            var lines = products.Select(p =>
            $"{p.PLU};{p.Name};{p.Price};{p.PriceUnit}");

            File.WriteAllLines(filePath, lines);
        }
    }
}

