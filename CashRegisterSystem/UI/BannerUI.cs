namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides visual banner elements for the application,
    /// including centered headers and a stylized logo.
    /// </summary>
    public static class BannerUI
    {
        public static void PrintHeader(string title)
        {
            Console.WriteLine();

            string headerLine = new string('=', Console.WindowWidth - 2);

            WriteCentered(headerLine);
            WriteCentered($"  {title}  ");
            WriteCentered(headerLine);

            Console.WriteLine();
        }

        public static void ShowLogo()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            // LOGON ÄR ORÖRD – exakt som du vill ha den
            Console.WriteLine("\t  🍎🍌🍇🍉 𝗙 𝗥 𝗨 𝗞 𝗧   𝗧 𝗢 𝗥 𝗚 𝗘 𝗧 🍓🍍🍒🍑");

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static string RepeatToWidth(string pattern, int width)
        {
            string result = "";
            while (result.Length < width)
                result += pattern;

            return result.Substring(0, width);
        }

        private static void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int leftPadding = Math.Max(0, (windowWidth - text.Length) / 2);

            Console.WriteLine(new string(' ', leftPadding) + text);
        }
    }
}