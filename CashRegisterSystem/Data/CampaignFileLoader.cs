using CashRegisterSystem.Models;

namespace CashRegisterSystem.Data
{
	public class CampaignFileLoader
	{
		private static readonly string CampaignFilePath = "campaigns.txt";

		public static List<Campaign> GetCampaigns()
		{
			if (!File.Exists(CampaignFilePath))
				return new List<Campaign>();

			// Lista som ska fyllas med Campaign-objekt
			var campaignListFromFile = new List<Campaign>();

			// Läser varje rad i filen och bygger Campaign-objekt
			foreach (var campaignLineText in File.ReadAllLines(CampaignFilePath))
			{
				// Dela upp raden i fält
				var campaignFieldsFromFile = campaignLineText.Split(';');

				// Skapa Campaign-objekt och lägg till i listan
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
			var campaignLinesToSave = campaigns.Select(c =>
				$"{c.Id};{c.Name};{c.Discount};{c.IsActive};{c.StartDate};{c.EndDate}");

			File.WriteAllLines(CampaignFilePath, campaignLinesToSave);
		}
	}
}
