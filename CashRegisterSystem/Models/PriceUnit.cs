namespace CashRegisterSystem.Models
{
    /// <summary>
    /// Defines how a product's price is measured.
    /// "Each" means price per item, and "PerKilo" means price per kilogram.
    /// </summary>
    public enum PriceUnit
    {
        Each,
        PerKilo
    }
}
