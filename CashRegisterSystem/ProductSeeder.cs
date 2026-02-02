
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public class ProductSeeder
    {
        public static List<ProductModel> GetProducts()
        {
            // PLU-lista Frukt & Grönt
            return new List<ProductModel>

            {
                new ProductModel { Id = "4011", Name = "Bananer", Price = 29.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "3214", Name = "Äpple", Price = 32.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "3271", Name = "Avokado", Price = 18.90m, PriceUnit = PriceUnit.Each },
                new ProductModel { Id = "3131", Name = "Kiwi", Price = 9.90m, PriceUnit = PriceUnit.Each },
                new ProductModel { Id = "3174", Name = "Apelsin", Price = 19.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "3220", Name = "Päron", Price = 28.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4251", Name = "Jordgubbar", Price = 35.09m, PriceUnit = PriceUnit.Each },
                new ProductModel { Id = "4521", Name = "Sparris", Price = 46.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4666", Name = "Lök gul", Price = 9.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4182", Name = "Lök Röd", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4688", Name = "Paprika röd", Price = 42.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4687", Name = "Paprika gul", Price = 46.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4686", Name = "Paprika grön", Price = 41.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4833", Name = "Potatis fast", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4863", Name = "Potatis mjölig", Price = 12.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4806", Name = "Tomat kvist", Price = 26.90m, PriceUnit = PriceUnit.PerKilo },
                new ProductModel { Id = "4251", Name = "Sallad isberg", Price = 24.40m, PriceUnit = PriceUnit.Each },
                new ProductModel { Id = "4645", Name = "Svamp champinjon", Price = 19.90m, PriceUnit = PriceUnit.Each },
                new ProductModel { Id = "3264", Name = "Vitlök", Price = 16.90m, PriceUnit = PriceUnit.Each },
            };
        }

        }
    }
