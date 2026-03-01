using System;

namespace CashRegisterSystem.UI
{
    public static class ConsoleLayout
    {
        public static void WriteCentered(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine();
                return;
            }

            int windowWidth = Console.WindowWidth;
            int leftPadding = Math.Max((windowWidth - text.Length) / 2, 0);

            Console.WriteLine(new string(' ', leftPadding) + text);
        }

        public static void WriteCenteredBlock(string title)
        {
            string horizontalLine = new string('=', Console.WindowWidth - 2);

            WriteCentered(horizontalLine);
            WriteCentered(title);
            WriteCentered(horizontalLine);
        }
    }
}