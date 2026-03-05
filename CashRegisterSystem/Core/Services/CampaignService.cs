using CashRegisterSystem.Data;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.Services
{
    /// <summary>
    /// Calculates campaign prices, discounts and multibuy totals for products.
    /// Keeps logic simple and prevents invalid values from causing crashes.
    /// </summary>
    public static class CampaignService
    {
        public static decimal GetPrice(Product p)
        {
            if (p == null)
                return 0;

            if (p.MultiBuyQuantity > 0 && p.MultiBuyPrice > 0)
                return p.Price;

            if (p.CampaignPrice > 0)
                return p.CampaignPrice;

            if (p.CampaignPercent > 0)
            {
                decimal discounted = p.Price * (1 - p.CampaignPercent / 100m);
                return Math.Round(discounted, 2);
            }

            return p.Price;
        }

        public static decimal GetMultiBuyTotal(Product p, decimal quantity)
        {
            if (p == null || quantity <= 0)
                return 0;

            if (p.MultiBuyQuantity <= 0 || p.MultiBuyPrice <= 0)
                return quantity * GetPrice(p);

            int fullPacks = (int)(quantity / p.MultiBuyQuantity);
            decimal rest = quantity % p.MultiBuyQuantity;

            decimal packTotal = fullPacks * p.MultiBuyPrice;
            decimal restTotal = rest * p.Price;

            return packTotal + restTotal;
        }

        public static decimal GetDiscount(Product p, decimal quantity)
        {
            if (p == null || quantity <= 0)
                return 0;

            decimal original = p.Price * quantity;

            if (p.MultiBuyQuantity > 0 && p.MultiBuyPrice > 0)
            {
                decimal multiTotal = GetMultiBuyTotal(p, quantity);
                return original - multiTotal;
            }

            if (p.CampaignPrice > 0)
            {
                decimal discounted = p.CampaignPrice * quantity;
                return original - discounted;
            }

            if (p.CampaignPercent > 0)
            {
                decimal discounted = GetPrice(p) * quantity;
                return original - discounted;
            }

            return 0;
        }

        public static string GetCampaignName(Product p)
        {
            if (p == null)
                return "Rabatt";

            if (p.MultiBuyQuantity > 0 && p.MultiBuyPrice > 0)
                return $"{p.MultiBuyQuantity} för {p.MultiBuyPrice:0.00} kr";

            if (p.CampaignPrice > 0)
                return $"Kampanjpris {p.CampaignPrice:0.00} kr";

            if (p.CampaignPercent > 0)
                return $"{p.CampaignPercent:0}% rabatt";

            return "Rabatt";
        }
    }
}
