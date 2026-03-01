namespace CashRegisterSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string PLU { get; set; } = "";
        public string Name { get; set; } = "";

        public decimal Price { get; set; }
        public PriceUnit PriceUnit { get; set; }

        // Kampanjnamn (gemensamt för alla typer)
        public string? CampaignName { get; set; }

        // Procentkampanj (t.ex. 20%)
        public decimal CampaignPercent { get; set; } = 0;

        // Fast kampanjpris (t.ex. 10 kr)
        public decimal CampaignPrice { get; set; } = 0;

        // Multiköp (t.ex. 3 för 20 kr)
        public int MultiBuyQuantity { get; set; } = 0;
        public decimal MultiBuyPrice { get; set; } = 0;

        // Datumstyrning
        public DateTime? CampaignStartDate { get; set; }
        public DateTime? CampaignEndDate { get; set; }

        // Aktiv/inaktiv
        public bool CampaignIsActive { get; set; } = true;

        // Fast moms
        public decimal VatRate => 0.12m;
    }
}