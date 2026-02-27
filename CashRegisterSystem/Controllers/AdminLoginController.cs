using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers
{
    public static class AdminLoginController
    {
        public static void Login()
        {
            Console.Clear();
            BannerUI.ShowLogo();

            Console.Write("Admin ID: ");
            string? adminId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(adminId))
            {
                Console.WriteLine("Admin ID får inte vara tomt.");
                Console.ReadKey();
                return;
            }

            Console.Write("Lösenord: ");
            string? adminPass = Console.ReadLine();

            if (adminPass?.ToLower() == "")
            {
                AdminMenuController.ShowAdminMenu();
            }
            else
            {
                Console.WriteLine("Tyvärr, lösenordet är 'admin'.");
                Console.ReadKey();
            }
        }
    }
}
