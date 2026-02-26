namespace CashRegisterSystem.Models
{
    /// <summary>
    /// Defines the pricing unit for a product, indicating whether the price 
    /// is applied per individual item or per kilogram.
    /// </summary>
    /// <remarks>
    /// This enum is used throughout the system to determine how a product's 
    /// price should be interpreted and calculated during checkout.
    /// </remarks>
    public enum PriceUnit
    {
        Each,
        PerKilo
    }
}