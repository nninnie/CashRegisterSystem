using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Core
{
    public class CartItem
    {
        public required  Product Product { get; set; }
        public decimal Quantity { get; set; }

        public decimal TotalPrice => Product.Price * Quantity;
    }
}