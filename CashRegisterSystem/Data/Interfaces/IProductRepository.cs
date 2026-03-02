using System.Collections.Generic;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Data.Interfaces
{
    /// <summary>
    /// Defines basic operations for loading and saving all products.
    /// Used so the system can switch storage method without changing logic.
    /// </summary>
    public interface IProductRepository
    {
        List<Product> GetAll();
        void SaveAll(List<Product> products);
    }
}
