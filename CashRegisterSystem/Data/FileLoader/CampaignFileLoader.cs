using CashRegisterSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CashRegisterSystem.Data
{
    /// <summary>
    /// Loads and saves campaign data for products.
    /// Ensures safe parsing and prevents crashes from invalid files.
    /// </summary>
    public class CampaignFileLoader
    {
        private static readonly string CampaignFilePath = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "Campaigns", "campaigns.txt");

        public static List<Product> LoadCampaignsIntoProducts(List<Product> products)
        {
            if (products == null)
                return new List<Product>();

            Directory.CreateDirectory(Path.GetDirectoryName(CampaignFilePath)!);

            if (!File.Exists(CampaignFilePath))
            {
                File.WriteAllText(CampaignFilePath, "");
                return products;
            }

            foreach (var line in File.ReadAllLines(CampaignFilePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');
                if (parts.Length != 7)
                    continue;

                string plu = parts[0];

                string campaignName = parts[1];

                if (!decimal.TryParse(parts[2], out decimal percent))
                    continue;

                if (!decimal.TryParse(parts[3], out decimal price))
                    continue;

                if (!int.TryParse(parts[4], out int multiQty))
                    continue;

                if (!decimal.TryParse(parts[5], out decimal multiPrice))
                    continue;

                if (!bool.TryParse(parts[6], out bool isActive))
                    continue;

                var product = products.FirstOrDefault(p => p.PLU == plu);
                if (product == null)
                    continue;

                product.CampaignName = campaignName;
                product.CampaignPercent = percent;
                product.CampaignPrice = price;
                product.MultiBuyQuantity = multiQty;
                product.MultiBuyPrice = multiPrice;
                product.CampaignIsActive = isActive;
            }

            return products;
        }
        public static void SaveCampaignsFromProducts(List<Product> products)
        {
            if (products == null)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(CampaignFilePath)!);

            var lines = products.Select(p =>
                $"{p.PLU};{p.CampaignName};{p.CampaignPercent};{p.CampaignPrice};{p.MultiBuyQuantity};{p.MultiBuyPrice};{p.CampaignIsActive}");

            File.WriteAllLines(CampaignFilePath, lines);
        }
    }
}
