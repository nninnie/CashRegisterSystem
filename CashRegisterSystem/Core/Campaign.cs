namespace CashRegisterSystem.Core
{
    /// <summary>
    /// Represents a marketing campaign with associated discount, status, and scheduling information.
    /// </summary>
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
