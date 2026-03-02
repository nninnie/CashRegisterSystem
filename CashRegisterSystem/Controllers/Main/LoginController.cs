using CashRegisterSystem.Data.Service;
using CashRegisterSystem.UI;
using System;

public static class LoginController
{
    /// <summary>
    /// Handles employee login and sets the logged-in state.
    /// Ensures safe input and prevents crashes from empty values.
    /// </summary>
    public static void Login(ref bool isLoggedIn)
    {
        while (true)
        {
            Console.Clear();
            BannerUI.ShowLogo();
            BannerUI.PrintHeader("LOGIN");

            Console.Write("Anställningsnummer: ");
            string employeeId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                ShowError("Fältet får inte vara tomt.");
                continue;
            }

            Console.Write("Lösenord: ");
            string code = Console.ReadLine() ?? "";

            // Temporary login logic
            if (string.Equals(code, "bajskorv", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(code))
            {
                LoginService.CurrentUserName = employeeId;
                isLoggedIn = true;
                return;
            }

            ShowError("Tyvärr, det är såklart 'bajskorv'!");
        }
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Thread.Sleep(900);
    }
}
