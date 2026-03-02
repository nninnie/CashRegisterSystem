using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;
using System;
using System.IO;

namespace CashRegisterSystem.Core.CheckoutService
{
    /// <summary>
    /// Handles payment selection, cash input, receipt creation and saving.
    /// Ensures safe input and prevents crashes during checkout.
    /// </summary>
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

            Console.WriteLine("\nVälj betalningsmetod:\n");

            int menuTop = Console.CursorTop;

            void DrawMenu()
            {
                Console.SetCursorPosition(0, menuTop);

                // Clear menu area
                Console.WriteLine(new string(' ', 40));
                Console.WriteLine(new string(' ', 40));

                Console.SetCursorPosition(0, menuTop);

                Console.WriteLine($"{(option == 1 ? "· " + highlight : "  ")}Kontant{reset}");
                Console.WriteLine($"{(option == 2 ? "· " + highlight : "  ")}Kort{reset}");
            }

            DrawMenu();

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow) option++;
                if (key.Key == ConsoleKey.UpArrow) option--;

                if (option < 1) option = 2;
                if (option > 2) option = 1;

                DrawMenu();

                if (key.Key == ConsoleKey.Enter)
                    break;
            }

            string paymentMethod = option == 1 ? "Kontant" : "Kort";
            decimal amountReceived = paymentMethod == "Kontant"
                ? ReadCashAmount()
                : 0;

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

            // Save by receipt number (not date)
            string filePath = Path.Combine(
                receiptFolder,
                $"RECEIPT_{receiptNumber}.txt");

            File.WriteAllText(filePath, receipt);

            Console.WriteLine("\nKvitto utskrivet till kund ✔️\n");
            Console.WriteLine("Tryck ENTER för nästa kund || ESC för att gå tillbaka till menyn.");

            var exitKey = Console.ReadKey(true);

            return exitKey.Key != ConsoleKey.Escape;
        }

        private decimal ReadCashAmount()
        {
            while (true)
            {
                Console.Write("Mottaget belopp: ");
                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out decimal amount) && amount >= 0)
                    return amount;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt belopp. Försök igen.\n");
                Console.ResetColor();
            }
        }
    }
}
