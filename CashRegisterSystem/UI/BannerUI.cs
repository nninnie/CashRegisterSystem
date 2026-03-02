namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides visual banner elements for the application,
    /// including centered headers and a stylized logo.
    /// </summary>
    public static class BannerUI
    {
        /// <summary>
        /// Prints a centered header with '=' lines above and below.
        /// Used for major sections such as "KASSA", "PLU-LISTA", etc.
        /// </summary>
        public static void PrintHeader(string title)
        {
            Console.WriteLine();

            string headerLine = new string('=', Console.WindowWidth - 2);

            WriteCentered(headerLine);
            WriteCentered($"  {title}  ");
            WriteCentered(headerLine);

            Console.WriteLine();
        }

        /// <summary>
        /// Displays the store logo in color.
        /// </summary>
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

        /// <summary>
        /// Writes text centered horizontally in the console window.
        /// </summary>
        private static void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int leftPadding = Math.Max(0, (windowWidth - text.Length) / 2);

            Console.WriteLine(new string(' ', leftPadding) + text);
        }
    }
}
