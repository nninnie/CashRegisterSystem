using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers.Admin
{
    public static class AdminLoginController
    {
        public static void Login()
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("ADMIN LOGIN");

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
