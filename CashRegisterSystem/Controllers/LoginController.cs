using CashRegisterSystem.Data.Service;
using CashRegisterSystem.UI;

public static class LoginController
{
    public static void Login(ref bool isLoggedIn)
    {
        while (true)
        {
            Console.Clear();
            BannerUI.PrintHeader("LOGIN");

            Console.Write("Anställningsnummer: ");
            string employeeId = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fältet får inte vara tomt.");
                Console.ResetColor();

                Thread.Sleep(900);
                continue; // ← hoppa tillbaka till början och rita om allt
            }

            Console.Write("Lösenord: ");
            string code = Console.ReadLine()!;

            if (code.ToLower() == "bajskorv" || code.ToLower() == "")
            {
                LoginService.CurrentUserName = employeeId;
                isLoggedIn = true;
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Tyvärr, det är såklart 'bajskorv'!");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
