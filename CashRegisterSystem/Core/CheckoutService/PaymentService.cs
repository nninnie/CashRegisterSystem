using CashRegisterSystem.Core.Services;
using CashRegisterSystem.Data.Service;
using CashRegisterSystem.Models;
using System;
using System.IO;

namespace CashRegisterSystem.Core.CheckoutService
{
    /// <summary>
    /// Handles payment selection, cash input, receipt creation and saving.
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

            int selectedOption = 1;
            const string highlight = "\u001b[32m";
            const string reset = "\u001b[0m";

            Console.WriteLine("\nVälj betalningsmetod:\n");
            int menuTopRow = Console.CursorTop;

            // Draws the payment menu safely at a fixed position
            void DrawPaymentMenu()
            {
                Console.SetCursorPosition(0, menuTopRow);

                for (int i = 0; i < 2; i++)
                {
                    Console.Write(new string(' ', 40));
                    Console.SetCursorPosition(0, menuTopRow + i);
                }

                Console.SetCursorPosition(0, menuTopRow);

                Console.WriteLine(selectedOption == 1
                    ? $"· {highlight}Kontant{reset}"
                    : "  Kontant");

                Console.WriteLine(selectedOption == 2
                    ? $"· {highlight}Kort{reset}"
                    : "  Kort");
            }

            // Draw menu once
            DrawPaymentMenu();

            // Menu navigation
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow) selectedOption++;
                if (key.Key == ConsoleKey.UpArrow) selectedOption--;

                if (selectedOption < 1) selectedOption = 2;
                if (selectedOption > 2) selectedOption = 1;

                Console.SetCursorPosition(0, menuTopRow);
                DrawPaymentMenu();

                if (key.Key == ConsoleKey.Enter)
                    break;
            }

            string paymentMethod = selectedOption == 1 ? "Kontant" : "Kort";

            // Read cash amount only for cash payments
            decimal amountReceived = paymentMethod == "Kontant"
                ? ReadCashAmount()
                : 0;

            // Calculate total to pay
            decimal totalToPay = 0;
            foreach (var item in cart.CartItems)
                totalToPay += item.TotalPrice - item.DiscountAmount;

            // If customer pays too little, ask again until enough
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
                    string input = Console.ReadLine() ?? "";

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

            // Receipt number
            int receiptNumber = ReceiptNumberService.GetNext();
            ReceiptNumberService.Save(receiptNumber);

            string cashierName = LoginService.CurrentUserName;
            int registerNumber = RegisterService.CurrentRegister;

            // Build receipt text
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

            // Save receipt folder
            string receiptFolder = Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "Receipts");

            Directory.CreateDirectory(receiptFolder);

            // Save as .txt
            string txtPath = Path.Combine(
                receiptFolder,
                $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            File.WriteAllText(txtPath, receiptText);

            // Save as .pdf (text-based)
            string pdfPath = Path.Combine(
                receiptFolder,
                $"RECEIPT_{DateTime.Now:yyyyMMdd}.pdf");

            File.WriteAllText(pdfPath, receiptText);

            Console.WriteLine("\nKvitto utskrivet till kund ✔️\n");
            Console.WriteLine("Tryck valfri tangent för att fortsätta.");
            Console.ReadKey(true);

            return false; // Always return a bool
        }

        /// <summary>
        /// Reads a valid non-negative cash amount from the console.
        /// </summary>
        private decimal ReadCashAmount()
        {
            while (true)
            {
                Console.Write("\nMottaget belopp: ");
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