namespace CashRegisterSystem.Models
{
    public class Product
    {
        public int Id { get; set; } // Reservfält för framtida databasstöd, används ej i denna version
        public required string PLU { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public PriceUnit PriceUnit { get; set; }
        public decimal VatRate => 0.12m; // 12% moms för livsmedel


    }
}
