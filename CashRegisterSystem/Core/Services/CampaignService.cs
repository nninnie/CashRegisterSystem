using CashRegisterSystem.Data;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.Services
{
    public static class CampaignService
    {
        // Hämtar rätt pris baserat på kampanjtyp
        public static decimal GetPrice(Product p)
        {
            // Multiköp hanteras i CartService (per antal)
            if (p.MultiBuyQuantity > 0 && p.MultiBuyPrice > 0)
                return p.Price;

            // Fast kampanjpris
            if (p.CampaignPrice > 0)
                return p.CampaignPrice;

            // Procentkampanj
            if (p.CampaignPercent > 0)
            {
                decimal discounted = p.Price * (1 - p.CampaignPercent / 100m);
                return Math.Round(discounted, 2);
            }

            // Ingen kampanj
            return p.Price;
        }

        // Beräknar totalpris för multiköp
        public static decimal GetMultiBuyTotal(Product p, decimal quantity)
        {
            if (p.MultiBuyQuantity <= 0 || p.MultiBuyPrice <= 0)
                return quantity * GetPrice(p);

            int fullPacks = (int)(quantity / p.MultiBuyQuantity);
            decimal rest = quantity % p.MultiBuyQuantity;

            decimal packTotal = fullPacks * p.MultiBuyPrice;
            decimal restTotal = rest * p.Price;

            return packTotal + restTotal;
        }

        // Beräknar rabattbeloppet
        public static decimal GetDiscount(Product p, decimal quantity)
        {
            decimal original = p.Price * quantity;

            // Multiköp
            if (p.MultiBuyQuantity > 0 && p.MultiBuyPrice > 0)
            {
                decimal multiTotal = GetMultiBuyTotal(p, quantity);
                return original - multiTotal;
            }

            // Fast kampanjpris
            if (p.CampaignPrice > 0)
            {
                decimal discounted = p.CampaignPrice * quantity;
                return original - discounted;
            }

            // Procentkampanj
            if (p.CampaignPercent > 0)
            {
                decimal discounted = GetPrice(p) * quantity;
                return original - discounted;
            }

            return 0;
        }

        // Genererar rabattnamn
        public static string GetCampaignName(Product p)
        {
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