using System;

namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides reusable layout helpers for the console UI.
    /// All methods scale automatically with the current console window width.
    /// This ensures a consistent and professional look across different screens.
    /// </summary>
    public static class ConsoleLayout
    {
        /// <summary>
        /// The standard width used for horizontal lines and blocks.
        /// Always adapts to the current console window size.
        /// </summary>
        public static int Width => Console.WindowWidth - 2;

        /// <summary>
        /// Writes text centered horizontally in the console.
        /// If the text is empty, a blank line is written.
        /// </summary>
        public static void WriteCentered(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine();
                return;
            }

            int leftPadding = Math.Max((Console.WindowWidth - text.Length) / 2, 0);
            Console.WriteLine(new string(' ', leftPadding) + text);
        }

        /// <summary>
        /// Writes a centered block with a title, framed by '=' lines.
        /// Used for major sections such as "KASSA", "MENY", etc.
        /// </summary>
        public static void WriteCenteredBlock(string title)
        {
            string line = new string('=', Width);

            WriteCentered(line);
            WriteCentered(title);
            WriteCentered(line);

            Console.WriteLine();
        }



        /// <summary>
        /// Writes a centered section title with visual markers.
        /// Used for subsections like "PLU-LISTA" or "KUNDVAGN".
        /// </summary>
        public static void WriteSectionTitle(string title)
        {
            Console.WriteLine();
            WriteCentered($"--- {title} ---");
            Console.WriteLine();
        }

        /// <summary>
        /// Writes text left-aligned.
        /// Used for lists, PLU rows, cart items, and other structured data.
        /// </summary>
        public static void WriteLeft(string text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        /// Writes a row of fixed-width columns.
        /// Each column automatically truncates long text and adds "..." if needed.
        /// Perfect for PLU lists and table-like layouts.
        /// </summary>
        public static void WriteColumns(params (string text, int width)[] columns)
        {
            string line = "";

            foreach (var col in columns)
            {
                // Truncate text if it exceeds the column width
                string t = col.text.Length > col.width
                    ? col.text.Substring(0, col.width - 3) + "..."
                    : col.text;

                // PadRight ensures stable column alignment
                line += t.PadRight(col.width);
            }

            Console.WriteLine(line);
        }
    }
}
