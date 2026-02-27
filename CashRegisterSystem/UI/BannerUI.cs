namespace CashRegisterSystem.UI
{
    /// <summary>
    //// Provides visual banner elements for the application, including 
    /// a fruit-themed header and a stylized logo. Handles Unicode output 
    /// and simple formatting to create decorative menu sections.
    /// </summary>
    public static class BannerUI
    {
        public static void PrintFruitHeader(string title, bool centered = false)
        {
            Console.WriteLine();

            string fruitsLeft = "    🍎🍌🍇🍉";
            string fruitsRight = "🍓🍍🍒🍑";

            string line = $"{fruitsLeft}  {title}  {fruitsRight}";

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            if (centered)
                WriteCentered(line);
            else
                Console.WriteLine(line);

            Console.ResetColor();
            Console.WriteLine();
        }


        private static string RepeatToWidth(string pattern, int width)
        {
            string result = "";
            while (result.Length < width)
                result += pattern;
            return result.Substring(0, width);
        }
        public static void ShowLogo()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t  🍎🍌🍇🍉 𝗙 𝗥 𝗨 𝗞 𝗧   𝗧 𝗢 𝗥 𝗚 𝗘 𝗧 🍓🍍🍒🍑");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WriteCentered(string text)
        {
            int width = Console.WindowWidth;
            int left = Math.Max(0, (width - text.Length) / 2);
            Console.WriteLine(new string(' ', left) + text);
        }
    }
}