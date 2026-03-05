namespace CashRegisterSystem.Models
{
    /// <summary>
    /// Represents a product in the cash register system.
    /// Stores PLU, name, price, price unit and VAT.
    /// Also supports different campaign types such as percent discount,
    /// fixed price, multibuy and date‑controlled campaigns.
    /// Price is always stored as an integer value to avoid invalid input.
    /// All fields are kept simple and safe to prevent crashes.
    /// </summary>

    public class Product
    {
        // Intended as an article number for future expansion
        // public int Id { get; set; } 
        public string PLU { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public PriceUnit PriceUnit { get; set; }
        public string? CampaignName { get; set; }
        public decimal CampaignPercent { get; set; } = 0;
        public decimal CampaignPrice { get; set; } = 0;
        public int MultiBuyQuantity { get; set; } = 0;
        public decimal MultiBuyPrice { get; set; } = 0;
        public DateTime? CampaignStartDate { get; set; }
        public DateTime? CampaignEndDate { get; set; }
        public bool CampaignIsActive { get; set; } = false;
        public decimal VatRate => 0.12m;
    }
}