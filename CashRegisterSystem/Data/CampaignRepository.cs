using CashRegisterSystem.Core;

namespace CashRegisterSystem.Data
{
    public class CampaignRepository
    {

        /// <summary>
        /// The file path to the text file that stores all campaign data.
        /// </summary>
        private static readonly string CampaignFilePath = "campaigns.txt";

        /// <summary>
        /// Retrieves all campaigns from the campaign data file.
        /// </summary>
        /// <remarks>The returned list reflects the current contents of the campaign data file at the time
        /// of the call. The order of campaigns in the list corresponds to their order in the file.</remarks>
        /// <returns>A list of <see cref="Campaign"/> objects representing all campaigns found in the data file. Returns an empty
        /// list if the file does not exist or contains no campaigns.</returns>
        public static List<Campaign> GetCampaigns()
        {
            var campaigns = new List<Campaign>();

            if (!File.Exists(CampaignFilePath))
                return campaigns;

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

        /// <summary>
        /// Saves the specified list of campaigns to a file, overwriting any existing campaign data.
        /// </summary>
        /// <remarks>Each campaign is serialized as a semicolon-delimited line in the file. The file is
        /// overwritten if it already exists.</remarks>
        /// <param name="campaigns">The list of campaigns to save. Each campaign in the list is written to the file as a separate line. Cannot
        /// be null.</param>
        public static void SaveCampaigns(List<Campaign> campaigns)
        {
            var campaignLinesToSave = campaigns.Select(c =>
                $"{c.Id};{c.Name};{c.Discount};{c.IsActive};{c.StartDate};{c.EndDate}");

            File.WriteAllLines(CampaignFilePath, campaignLinesToSave);
        }
    }
}
