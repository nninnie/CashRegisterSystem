namespace CashRegisterSystem.UI
{
    public static class AdminUI
    {
        public static void ShowConfirmation(string title, string details)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{title}");
            Console.ResetColor();

            Console.WriteLine(details);

        }

        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
