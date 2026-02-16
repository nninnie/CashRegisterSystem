namespace CashRegisterSystem.Data
{
    public class ReceiptCounter
    {
        // Filen som sparar senaste kvittonumret.
        // Tack vare filen fortsätter numret även när programmet startas om.
        private const string ReceiptNumberFilePath = "receipt_number.txt";

        public int GetNextReceiptNumber()
        {
            int nextReceiptNumber = 1; // Starta på 1 om filen inte finns

            // Om filen finns → läs senaste kvittonumret
            if (File.Exists(ReceiptNumberFilePath))
            {
                using (var reader = new StreamReader(ReceiptNumberFilePath))
                {
                    // Läs talet som ligger i filen (t.ex. "17")
                    string receiptNumberInFile = reader.ReadToEnd();

                    // Om talet är giltigt → öka med 1
                    if (int.TryParse(receiptNumberInFile, out int lastNumber))
                    {
                        nextReceiptNumber = lastNumber + 1;
                    }
                }
            }

            // Skriv det nya kvittonumret tillbaka till filen
            // (så nästa gång programmet startar fortsätter numret)
            using (var writer = new StreamWriter(ReceiptNumberFilePath, false))
            {
                writer.Write(nextReceiptNumber);
            }

            return nextReceiptNumber; // Skicka tillbaka nästa kvittonummer
        }
    }
}
