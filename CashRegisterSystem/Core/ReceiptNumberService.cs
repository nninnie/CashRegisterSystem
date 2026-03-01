using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Core
{
    namespace CashRegisterSystem.Data.Service
    {
        public static class ReceiptNumberService
        {
            private static readonly string CounterFile =
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Receipts", "receipt_counter.txt");

            public static int GetNext()
            {
                if (!File.Exists(CounterFile))
                    return 1;

                string text = File.ReadAllText(CounterFile);
                return int.TryParse(text, out int num) ? num + 1 : 1;
            }

            public static void Save(int number)
            {
                File.WriteAllText(CounterFile, number.ToString());
            }
        }
    }

}
