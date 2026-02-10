using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem
{
    public static class ProductAdminService
    {
        public static int ShowAdminMenu(List<ProductModel> products)
        {
            Console.Clear();
            Console.WriteLine("=== ADMIN MENY ===)");
            Console.WriteLine("1. Ändra produkt");
            Console.WriteLine("2. Lägg till produkt");
            Console.WriteLine("3. Ta bort produkt");
            Console.WriteLine("4. Tillbaka");
            Console.Write("\nVal: ");

            string? usersAdminInput = Console.ReadLine();


            if (!int.TryParse(usersAdminInput, out int menuChoice))
            {
                Console.WriteLine("Fel menyval, försök igen.");
                Console.ReadKey();
                Console.Clear();
                return -1;
            }
            return menuChoice;

            // Här kommer admin-logik senare
        }
    }
}
