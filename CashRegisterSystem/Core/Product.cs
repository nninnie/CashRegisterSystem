namespace CashRegisterSystem.Core
{
    /// <summary>
    /// Represents a product that can be sold in the cash register system. 
    /// Each product is uniquely identified by its PLU code, which is used 
    /// throughout the application for lookups and transactions.
    /// </summary>
    /// <remarks>
    /// The <c>Id</c> property is included as a reserved field for potential 
    /// future database integration, but is not used in the current file‑based 
    /// implementation. The PLU code serves as the primary identifier in this version.
    /// </remarks>
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
