using System;
using System.Collections.Generic;
using System.Linq;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.CheckoutService
{
    public class DiscountService
    {
        public decimal CalculateDiscount(Product product, decimal quantity, List<Campaign> campaigns)
        {
            var validCampaigns = campaigns
                .Where(c => c.IsActive &&
                            c.StartDate <= DateTime.Today &&
                            c.EndDate >= DateTime.Today)
                .ToList();

            if (!validCampaigns.Any())
                return 0;

            var campaign = validCampaigns
                .FirstOrDefault(c => c.Name == product.Name);

            if (campaign == null)
                return 0;

            decimal totalPrice = product.Price * quantity;

            return totalPrice * (campaign.Discount / 100);
        }
    }
}
