using CashRegisterSystem.Models;

namespace CashRegisterSystem.Data
{
	public class CampaignFileLoader
	{
		private static readonly string CampaignFilePath = Path.Combine(
			AppContext.BaseDirectory, "..", "..", "..", "Campaigns", "campaigns.txt");



        public static List<Campaign> GetCampaigns()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CampaignFilePath)!);

            if (!File.Exists(CampaignFilePath))
            {
                File.WriteAllText(CampaignFilePath, "");
                return new List<Campaign>();
            }

            var campaignList = new List<Campaign>();

            foreach (var line in File.ReadAllLines(CampaignFilePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');

                if (parts.Length != 6)
                    continue; // hoppa över korrupt rad

                if (!int.TryParse(parts[0], out int id)) continue;
                string name = parts[1];
                if (!decimal.TryParse(parts[2], out decimal discount)) continue;
                if (!bool.TryParse(parts[3], out bool isActive)) continue;
                if (!DateTime.TryParse(parts[4], out DateTime start)) continue;
                if (!DateTime.TryParse(parts[5], out DateTime end)) continue;

                campaignList.Add(new Campaign
                {
                    Id = id,
                    Name = name,
                    Discount = discount,
                    IsActive = isActive,
                    StartDate = start,
                    EndDate = end
                });
            }

            return campaignList;
        }




        public static void SaveCampaigns(List<Campaign> campaigns)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(CampaignFilePath)!);

            var campaignLinesToSave = campaigns.Select(c =>
                $"{c.Id};{c.Name};{c.Discount};{c.IsActive};{c.StartDate:yyyy-MM-dd};{c.EndDate:yyyy-MM-dd}");

            File.WriteAllLines(CampaignFilePath, campaignLinesToSave);
		}
	}
}