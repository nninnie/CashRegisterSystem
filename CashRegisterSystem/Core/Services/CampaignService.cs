using CashRegisterSystem.Data;
using CashRegisterSystem.Models;
using System.Linq;

namespace CashRegisterSystem.Core.Services
{
    public static class CampaignService
    {
        public static decimal GetPrice(Product product)
        {
            var campaigns = CampaignFileLoader.GetCampaigns();
            var active = campaigns.FirstOrDefault(c =>
                c.Name == product.Name &&
                c.IsActive &&
                c.StartDate <= DateTime.Today &&
                c.EndDate >= DateTime.Today);

            if (active == null)
                return product.Price;

            decimal discounted = product.Price * (1 - active.Discount / 100m);
            return Math.Round(discounted, 2);
        }
    }
}
