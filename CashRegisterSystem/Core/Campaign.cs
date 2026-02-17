using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Core
{
    public class Campaign
    {
        public int Id { get; set;  }
        public string Name { get; set; } = ""; // Default värde, tar bort varning null
        public decimal Discount { get; set; }  
        public bool IsActive { get; set; }  = true; // soft delete
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
