using System;
using System.IO;

namespace CashRegisterSystem.Data.Service
{
    /// <summary>
    /// Handles the receipt counter stored in a text file.
    /// Ensures safe reading and writing so the system never crashes.
    /// </summary>
    public static class ReceiptNumberService
    {
        private static readonly string CounterFile =
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Receipts", "receipt_counter.txt");

        public static int GetNext()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CounterFile)!);

            if (!File.Exists(CounterFile))
                return 1;

            string text = File.ReadAllText(CounterFile);

            return int.TryParse(text, out int num)
                ? num + 1
                : 1; // Prevents crashes from corrupted files
        }

        public static void SaveCurrentReceiptNumber(int number)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CounterFile)!);

            if (number < 1)
                number = 1;

            File.WriteAllText(CounterFile, number.ToString());
        }
    }
}
