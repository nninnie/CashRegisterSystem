using CashRegisterSystem.Core.Services;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin
{
    /// <summary>
    /// Handles admin login and grants access to the admin menu.
    /// Ensures safe input and consistent UI behavior.
    /// </summary>
    public static class AdminLoginController
    {
        public static void Login()
        {
            while (true)
            {
                Console.Clear();
                BannerUI.ShowLogo();
                BannerUI.PrintHeader("ADMIN LOGIN");

                string? adminId = InputService.ReadStringAllowEmpty("Admin ID: ");

                if (string.IsNullOrWhiteSpace(adminId))
                {
                    ShowError("Fältet får inte vara tomt.");
                    continue;
                }

                string? adminPass = InputService.ReadStringAllowEmpty("Lösenord: ");

                // Temporär inloggning
                if (string.Equals(adminPass, "admin", StringComparison.OrdinalIgnoreCase) ||
                    string.IsNullOrWhiteSpace(adminPass))
                {
                    AdminMenuController.ShowAdminMenu();
                    return;
                }

                ShowError("Tyvärr, lösenordet är 'admin'.");
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
}
