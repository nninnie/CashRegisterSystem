using CashRegisterSystem.Data.Service;
using CashRegisterSystem.UI;

namespace CashRegisterSystem.Controllers
{
    public static class LoginController
    {
        public static void Login(ref bool isLoggedIn)
        {
            Console.Clear();
            BannerUI.PrintFruitHeader("LOGIN");

            Console.Write("Anställningsnummer: ");
            string? employeeId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                Console.WriteLine("Anställningsnummer får inte vara tomt.");
                Console.ReadKey();
                return;
            }

            Console.Write("Lösenord: ");
            string? code = Console.ReadLine();

            if (code?.ToLower() == "")
            {
                LoginService.CurrentUserName = employeeId;
                isLoggedIn = true;
            }
            else
            {
                Console.WriteLine("Tyvärr, det är såklart 'bajskorv'!");
                Console.ReadKey();
            }
        }
    }
}
