using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Core
{
    public class Product
    {
        // Beskriver en produkt i kassan
        public required string Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public PriceUnit PriceUnit { get; set; }

    }
}
