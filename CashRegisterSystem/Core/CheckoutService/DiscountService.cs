using System;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.CheckoutService
{
    public class DiscountService
    {
        public decimal CalculateDiscount(Product product, decimal quantity)
        {
            if (!product.CampaignIsActive)
                return 0;

            if (product.CampaignStartDate.HasValue && product.CampaignStartDate > DateTime.Today)
                return 0;

            if (product.CampaignEndDate.HasValue && product.CampaignEndDate < DateTime.Today)
                return 0;

            decimal totalPrice = product.Price * quantity;

            // Procentkampanj
            if (product.CampaignPercent > 0)
            {
                return totalPrice * (product.CampaignPercent / 100);
            }

            // Fast kampanjpris
            if (product.CampaignPrice > 0)
            {
                decimal normalPrice = totalPrice;
                decimal campaignPrice = product.CampaignPrice * quantity;
                return normalPrice - campaignPrice;
            }

            // Multiköp (t.ex. 3 för 20 kr)
            if (product.MultiBuyQuantity > 0 && product.MultiBuyPrice > 0)
            {
                int fullSets = (int)(quantity / product.MultiBuyQuantity);
                decimal remaining = quantity % product.MultiBuyQuantity;

                decimal normalPrice = totalPrice;
                decimal multiBuyTotal =
                    (fullSets * product.MultiBuyPrice) +
                    (remaining * product.Price);

                return normalPrice - multiBuyTotal;
            }

            return 0;
        }
    }
}