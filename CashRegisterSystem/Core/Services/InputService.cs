namespace CashRegisterSystem.Core.Services
{
    public static class InputService
    {
        // STRIKT STRING – används i menyer, admin, produktinmatning
        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            return input.Trim();
        }

        // TILLÅTER TOM STRÄNG – används i LOGIN och ADMIN LOGIN
        public static string ReadStringAllowEmpty(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        // HELTAL
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int value))
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt tal. Försök igen.");
                Console.ResetColor();
            }
        }

        // DECIMAL
        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out decimal value))
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt belopp. Försök igen.");
                Console.ResetColor();
            }
        }

        // DATUM – används i kampanjer
        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (DateTime.TryParse(input, out DateTime date))
                    return date;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt datum. Ange i formatet ÅÅÅÅ-MM-DD.");
                Console.ResetColor();
            }
        }
    }
}
