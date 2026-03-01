using CashRegisterSystem.UI;
using CashRegisterSystem.Core.Services;

namespace CashRegisterSystem.Controllers.Admin
{
    public static class AdminLoginController
    {
        public static void Login()
        {
            while (true)
            {
                Console.Clear();
                BannerUI.PrintHeader("ADMIN LOGIN");

                string adminId = InputService.ReadStringAllowEmpty("Admin ID: ");

                if (string.IsNullOrWhiteSpace(adminId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fältet får inte vara tomt.");
                    Console.ResetColor();

                    Thread.Sleep(900);
                    continue; // ← hoppa tillbaka till toppen
                }

                string adminPass = InputService.ReadStringAllowEmpty("Lösenord: ");

                //if (string.IsNullOrWhiteSpace(adminPass))
                //{
                //    Console.ForegroundColor = ConsoleColor.Red;
                //    Console.WriteLine("Fältet får inte vara tomt.");
                //    Console.ResetColor();

                //    Thread.Sleep(900);
                //    continue; // ← hoppa tillbaka till toppen
                //}

                if (adminPass.ToLower() == "admin" || adminPass.ToLower() == "")
                {
                    AdminMenuController.ShowAdminMenu();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tyvärr, lösenordet är 'admin'.");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }
}
