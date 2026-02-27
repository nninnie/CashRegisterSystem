using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterSystem.Data.Service
{
    public static class LoginService
    {
        //Tills någon loggar in på en kassa
        public static string CurrentUserName { get; set; } = "Okänd"; 
    }
}
