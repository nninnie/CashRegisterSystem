using System;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides reusable banner and logo rendering for all menu screens.
    /// Ensures consistent width and centered text across the application.
    /// </summary>
    public static class BannerUI
    {
        // Fixed width used for all banners to match checkout layout
        private const int BannerWidth = 70;

        /// <summary>
        /// Prints a clean, full-width header with centered title.
        /// </summary>
        public static void PrintHeader(string title)
        {
            Console.WriteLine();

            string line = new string('=', BannerWidth);

            // Full-width top border
            Console.WriteLine(line);

            // Centered title
            int padding = (BannerWidth - title.Length) / 2;
            Console.WriteLine(new string(' ', padding) + title.ToUpper());

            // Full-width bottom border
            Console.WriteLine(line);

            Console.WriteLine();
        }


        /// <summary>
        /// Displays the store logo centered in the same width as banners.
        /// </summary>
        public static void ShowLogo()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            // Exact visual centering for Unicode/emoji logo
            string logo = "🍎🍌🍇🍉 𝗙 𝗥 I U 𝗧   M A R K E 𝗧 🍓🍍🍒🍑";
            string paddedLogo = new string(' ', 16) + logo; 

            Console.WriteLine(paddedLogo);

            Console.ResetColor();
            Console.WriteLine();
            
        }


        /// <summary>
        /// Writes a single line of text centered within the banner width.
        /// </summary>
        private static void WriteCentered(string text)
        {
            int padding = Math.Max(0, (BannerWidth - text.Length) / 2);
            Console.WriteLine(new string(' ', padding) + text);
        }
    }
}
