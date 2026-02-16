namespace CashRegisterSystem.Data
{
    public class ReceiptCounter
    {
        private const string FilePath = "receipt_number.txt";

        public int GetNextReceiptNumber()
        {
            int nextReceiptNumber = 1;

            // Läs senaste numret om filen finns
            if (File.Exists(FilePath))
            {
                using (var reader = new StreamReader(FilePath))
                {
                    string line = reader.ReadToEnd();

                    if (int.TryParse(line, out int lastNumber))
                    {
                        nextReceiptNumber = lastNumber + 1;
                    }
                }
            }
            // Skriv tillbaka det nya numret
            using (var writer = new StreamWriter(FilePath, false))
            {
                writer.Write(nextReceiptNumber);
            }
            return nextReceiptNumber;
        }
    }
}
