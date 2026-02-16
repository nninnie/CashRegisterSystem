namespace CashRegisterSystem.Core
{
    public class Product
    {
        // Beskriver en produkt i kassan
        public int Id { get; set; }
        public required string PLU { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public PriceUnit PriceUnit { get; set; }

    }
}
