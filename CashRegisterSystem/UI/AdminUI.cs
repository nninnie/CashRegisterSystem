namespace CashRegisterSystem.UI
{
    /// <summary>
    /// Provides simple visual feedback messages for admin actions,
    /// such as confirmations and error notifications.
    /// </summary>
    public static class AdminUI
    {
        /// <summary>
        /// Shows a confirmation message in green text.
        /// Used when an admin action succeeds.
        /// </summary>
        public static void ShowConfirmation(string title, string details)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{title}");
            Console.ResetColor();

            Console.WriteLine(details);
        }

        /// <summary>
        /// Shows an error message in red text.
        /// Used when an admin action fails or invalid input is given.
        /// </summary>
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
