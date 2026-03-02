using System;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.CheckoutService
{
    /// <summary>
    /// Calculates discount amounts for products based on active campaigns.
    /// Handles percent discounts, fixed prices and multibuy offers safely.
    /// </summary>
    public class DiscountService
    {
        public decimal CalculateDiscount(Product product, decimal quantity)
        {
            if (product == null || quantity <= 0)
                return 0;

            if (!product.CampaignIsActive)
                return 0;

            // Check campaign date range
            if (product.CampaignStartDate.HasValue && product.CampaignStartDate > DateTime.Today)
                return 0;

            if (product.CampaignEndDate.HasValue && product.CampaignEndDate < DateTime.Today)
                return 0;

            decimal originalTotal = product.Price * quantity;

            // Percent discount
            if (product.CampaignPercent > 0)
            {
                decimal discount = originalTotal * (product.CampaignPercent / 100m);
                return Math.Round(discount, 2);
            }

            // Fixed campaign price
            if (product.CampaignPrice > 0)
            {
                decimal campaignTotal = product.CampaignPrice * quantity;
                return originalTotal - campaignTotal;
            }

            // Multibuy (e.g., 3 for 20 kr)
            if (product.MultiBuyQuantity > 0 && product.MultiBuyPrice > 0)
            {
                int fullSets = (int)(quantity / product.MultiBuyQuantity);
                decimal remaining = quantity % product.MultiBuyQuantity;

                decimal multiBuyTotal =
                    (fullSets * product.MultiBuyPrice) +
                    (remaining * product.Price);

                return originalTotal - multiBuyTotal;
            }

            return 0;
        }
    }
}
