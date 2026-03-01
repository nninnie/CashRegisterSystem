using CashRegisterSystem.Core.CashRegisterSystem.Data.Service;
using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data;
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

                // Rensa två rader
                Console.WriteLine(new string(' ', 40));
                Console.WriteLine(new string(' ', 40));

                // Flytta tillbaka markören
                Console.SetCursorPosition(0, menuTop);

                // Rita menyn igen
                Console.WriteLine($"{(option == 1 ? "· " + highlight : "  ")}Kontant{reset}");
                Console.WriteLine($"{(option == 2 ? "· " + highlight : "  ")}Kort{reset}");
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
                amountReceived = InputService.ReadDecimal("\nMottaget belopp: ");
            }

            int receiptNumber = ReceiptNumberService.GetNext();
            ReceiptNumberService.Save(receiptNumber);

            string cashierName = LoginService.CurrentUserName;
            int registerNumber = RegisterService.CurrentRegister;

            string receipt = ReceiptUI.FormatReceipt(
                receiptNumber,
                cart.CartItems,
                cashierName,
                registerNumber,
                paymentMethod,
                amountReceived
            );

            Console.Clear();
            Console.WriteLine(receipt);

            string receiptFolder = Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "Receipts");

            Directory.CreateDirectory(receiptFolder);

            string filePath = Path.Combine(
                receiptFolder,
                $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            File.AppendAllText(filePath,
                $"\n=== KVITTO {receiptNumber} ===\n" +
                receipt +
                "\n=== SLUT ===\n");

            Console.WriteLine("\nKvitto utskrivet till kund ✔️\n");
            Console.WriteLine("Tryck ENTER för nästa kund || ESC för att gå tillbaka till menyn.");

            var exitKey = Console.ReadKey(true);

            if (exitKey.Key == ConsoleKey.Escape)
                return false;

            return true;
        }
    }
}
