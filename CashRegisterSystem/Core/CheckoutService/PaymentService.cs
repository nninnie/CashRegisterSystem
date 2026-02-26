using CashRegisterSystem.Data;
using CashRegisterSystem.Data.Interfaces;
using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;

namespace CashRegisterSystem.Core.CheckoutService
{
    public class PaymentService
    {
        public bool CompletePayment(Cart cart)
        {
            if (cart == null || cart.CartItems.Count == 0)
            {
                Console.WriteLine("Kundvagnen är tom.");
                return true;
            }

            int option = 1;
            const string highlight = "\u001b[32m";
            const string reset = "\u001b[0m";
            ConsoleKeyInfo key;

            Console.WriteLine("\nVälj betalningsmetod:\n");

            int menuTop = Console.CursorTop;

            void DrawMenu()
            {
                Console.SetCursorPosition(0, menuTop);

                Console.WriteLine($"{(option == 1 ? "· " + highlight : "  ")}Kontant{reset}   ");
                Console.WriteLine($"{(option == 2 ? "· " + highlight : "  ")}Kort{reset}      ");
            }

            DrawMenu();

            while (true)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow) option++;
                if (key.Key == ConsoleKey.UpArrow) option--;

                if (option < 1) option = 2;
                if (option > 2) option = 1;

                DrawMenu(); 

                if (key.Key == ConsoleKey.Enter)
                    break;
            }


            string paymentMethod = option == 1 ? "Kontant" : "Kort";
            decimal amountReceived = 0;

            if (paymentMethod == "Kontant")
            {
                Console.Write("\nMottaget belopp: ");
                amountReceived = decimal.Parse(Console.ReadLine()!);
            }


            // Hämta info från systemet
            var counter = new ReceiptCounter();
            int receiptNumber = counter.GetNextReceiptNumber();
            string cashierName = LoginService.CurrentUserName;
            int registerNumber = RegisterService.CurrentRegister;

            // Skapa kvittot
            string receipt = ReceiptUI.FormatReceipt(
                receiptNumber,
                cart.CartItems,
                cashierName,
                registerNumber,
                paymentMethod,
                amountReceived
            );

            // Skriv ut kvittot
            Console.Clear();
            Console.WriteLine(receipt);

            // Spara kvittot i en fil
            string receiptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Receipts", "receipt.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(receiptPath)!);

            if (!File.Exists(receiptPath))
                File.WriteAllText(receiptPath, ""); // skapa filen

            File.AppendAllText(receiptPath, receipt + "\n\n");

            Console.WriteLine("\nKvitto utskrivet till kund ✔️\n");

            Console.WriteLine("Tryck ENTER för nästa kund || ESC för att gå tillbaka till menyn.");

            var exitKey = Console.ReadKey(true);

            if (exitKey.Key == ConsoleKey.Escape)
                return false;

            return true;
        }

    }
}
