using CashRegisterSystem.Data;
using CashRegisterSystem.Data.Interfaces;
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

            Console.WriteLine("\nVälj betalningsmetod:");
            Console.WriteLine("1. Kontant");
            Console.WriteLine("2. Kort");
            Console.Write("Val: ");

            string choice = Console.ReadLine()!;
            string paymentMethod = "";
            decimal amountReceived = 0;

            if (choice == "1")
            {
                paymentMethod = "Kontant";
                Console.Write("Mottaget belopp: ");
                amountReceived = decimal.Parse(Console.ReadLine()!);
            }
            else
            {
                paymentMethod = "Kort";
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

            Console.WriteLine("\nKvitto utskrivet till kund ✔️\n");

            Console.WriteLine("Tryck ENTER för nästa kund eller ESC för att gå tillbaka till menyn.");

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Escape)
                return false;

            return true;
        }

    }
}
