using CashRegisterSystem.Models;

namespace CashRegisterSystem.Data
{
    public class CampaignFileLoader
    {
        private static readonly string CampaignFilePath = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "Campaigns", "campaigns.txt");

        public static List<Product> LoadCampaignsIntoProducts(List<Product> products)
        {
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
                decimal percent = decimal.Parse(parts[2]);
                decimal price = decimal.Parse(parts[3]);
                int multiQty = int.Parse(parts[4]);
                decimal multiPrice = decimal.Parse(parts[5]);
                bool isActive = bool.Parse(parts[6]);

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
            Directory.CreateDirectory(Path.GetDirectoryName(CampaignFilePath)!);

            var lines = products.Select(p =>
                $"{p.PLU};{p.CampaignName};{p.CampaignPercent};{p.CampaignPrice};{p.MultiBuyQuantity};{p.MultiBuyPrice};{p.CampaignIsActive}");

            File.WriteAllLines(CampaignFilePath, lines);
        }
    }
}