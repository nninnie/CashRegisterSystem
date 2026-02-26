using System.Collections.Generic;
using CashRegisterSystem.Core;

namespace CashRegisterSystem.Data.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        void SaveAll(List<Product> products);
    }
}
