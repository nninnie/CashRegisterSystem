using System;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides reusable banner and logo rendering for all menu screens.
    /// Ensures consistent width and centered text across the application.
    /// </summary>
    public static class BannerUI
    {
        private const int BannerWidth = 70;

        /// <summary>
        /// Prints a clean, full-width header with centered title.
        /// </summary>
        public static void PrintHeader(string title)
        {
            Console.WriteLine();

            string line = new string('=', BannerWidth);

            Console.WriteLine(line);

            int padding = (BannerWidth - title.Length) / 2;
            Console.WriteLine(new string(' ', padding) + title.ToUpper());

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

            string logo = "🍎🍌🍇🍉 𝗙 𝗥 U K T   M A R K N A D E N 🍓🍍🍒🍑";
            string paddedLogo = new string(' ', 16) + logo; 

            Console.Write(paddedLogo);

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();

        }
    }
}
