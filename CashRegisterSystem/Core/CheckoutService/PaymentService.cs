using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;
using CashRegisterSystem.UI;
using System;
using System.IO;

namespace CashRegisterSystem.Core.CheckoutService
{
    /// <summary>
    /// Handles payment selection, cash input, receipt creation and saving.
    /// </summary>
    public class PaymentService
    {
        /// <summary>
        /// Completes the payment flow for the current cart.
        /// </summary>
        public bool CompletePayment(Cart cart)
        {

            if (cart == null || cart.CartItems.Count == 0)
            {
                Console.WriteLine("Kundvagnen är tom.");
                return true;
            }

            int selectedOption = 1;
            const string highlight = "\u001b[32m";
            const string reset = "\u001b[0m";

            void DrawPaymentMenu(int startRow)
            {
                // Make sure this value stays within the allowed range so it doesn’t overflow or break the layout.
                if (startRow < 0) startRow = 0;
                if (startRow >= Console.BufferHeight - 2) startRow = Console.BufferHeight - 3;

                Console.SetCursorPosition(0, startRow);
                Console.Write(new string(' ', Console.WindowWidth));

                Console.SetCursorPosition(0, startRow + 1);
                Console.Write(new string(' ', Console.WindowWidth));

                Console.SetCursorPosition(0, startRow);
                Console.WriteLine(selectedOption == 1
                    ? $"· {highlight}Kontant{reset}"
                    : "  Kontant");

                Console.SetCursorPosition(0, startRow + 1);
                Console.WriteLine(selectedOption == 2
                    ? $"· {highlight}Kort{reset}"
                    : "  Kort");
            }

            Console.WriteLine("\nBetalningsmetod:\n");
            Console.WriteLine();

            int menuStartRow = Console.CursorTop;

            DrawPaymentMenu(menuStartRow);

            bool paymentChosen = false;

            while (!paymentChosen)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        selectedOption = selectedOption == 2 ? 1 : selectedOption + 1;
                        break;

                    case ConsoleKey.UpArrow:
                        selectedOption = selectedOption == 1 ? 2 : selectedOption - 1;
                        break;

                    case ConsoleKey.Enter:
                        paymentChosen = true;
                        continue;
                }

                DrawPaymentMenu(menuStartRow);
            }


            string paymentMethod = selectedOption == 1 ? "Kontant" : "Kort";

            decimal amountReceived = paymentMethod == "Kontant"
                ? ReadCashAmount()
                : 0;

            decimal totalToPay = 0;
            foreach (var item in cart.CartItems)
            {
                totalToPay += item.TotalPrice - item.DiscountAmount;
            }

            if (paymentMethod == "Kontant")
            {
                while (amountReceived < totalToPay)
                {
                    decimal missing = totalToPay - amountReceived;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nKunden saknar {missing:0.00} kr.");
                    Console.WriteLine("Ta emot resterande belopp.\n");
                    Console.ResetColor();

                    Console.Write("Mottaget belopp: ");
                    string input = Console.ReadLine() ?? string.Empty;

                    if (decimal.TryParse(input, out decimal extra) && extra >= 0)
                    {
                        amountReceived += extra;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt belopp. Försök igen.\n");
                        Console.ResetColor();
                    }
                }
            }

            int receiptNumber = ReceiptNumberService.GetNext();
            ReceiptNumberService.SaveCurrentReceiptNumber(receiptNumber);

            string cashierName = LoginService.CurrentUserName;
            int registerNumber = RegisterService.CurrentRegister;

            string receiptText = ReceiptUI.FormatReceipt(
                receiptNumber,
                cart.CartItems,
                cashierName,
                registerNumber,
                paymentMethod,
                amountReceived
            );

            Console.Clear();
            Console.WriteLine(receiptText);

            string receiptFolder = Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "Receipts");

            Directory.CreateDirectory(receiptFolder);

            string txtPath = Path.Combine(
                receiptFolder,
                $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            File.WriteAllText(txtPath, receiptText);

            Console.WriteLine("\nKvitto utskrivet till kund ✔️\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("ENTER för nästa kund || ESC för att gå till menyn");
            Console.ResetColor();
            var keys = Console.ReadKey(true);

            if (keys.Key == ConsoleKey.Enter)
                return false;  

            if (keys.Key == ConsoleKey.Escape)
                return true;    

            return false;      

        }

        /// <summary>
        /// Reads a valid non-negative cash amount from the console.
        /// </summary>
        private decimal ReadCashAmount()
        {
            while (true)
            {
                Console.Write("Mottaget belopp: ");
                string input = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(input, out decimal amount) && amount >= 0)
                {
                    return amount;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt belopp. Försök igen.\n");
                Console.ResetColor();
            }
        }
    }
}
