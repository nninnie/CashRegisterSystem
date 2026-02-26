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
				Directory.CreateDirectory(Path.GetDirectoryName(CampaignFilePath)!);
				File.WriteAllText(CampaignFilePath, ""); // skapa tom fil
				return new List<Campaign>();
			}


			var campaignListFromFile = new List<Campaign>();


			foreach (var campaignLineText in File.ReadAllLines(CampaignFilePath))
			{
				var campaignFieldsFromFile = campaignLineText.Split(';');

				campaignListFromFile.Add(new Campaign
				{
					Id = int.Parse(campaignFieldsFromFile[0]),
					Name = campaignFieldsFromFile[1],
					Discount = decimal.Parse(campaignFieldsFromFile[2]),
					IsActive = bool.Parse(campaignFieldsFromFile[3]),
					StartDate = DateTime.Parse(campaignFieldsFromFile[4]),
					EndDate = DateTime.Parse(campaignFieldsFromFile[5])
				});
			}

			return campaignListFromFile;
		}



		public static void SaveCampaigns(List<Campaign> campaigns)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(CampaignFilePath)!);

			var campaignLinesToSave = campaigns.Select(c =>
				$"{c.Id};{c.Name};{c.Discount};{c.IsActive};{c.StartDate};{c.EndDate}");

			File.WriteAllLines(CampaignFilePath, campaignLinesToSave);
		}
	}
}