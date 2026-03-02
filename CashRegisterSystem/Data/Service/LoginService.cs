using System;

namespace CashRegisterSystem.Data.Service
{
    /// <summary>
    /// Stores the name of the currently logged-in user.
    /// Used so the system always knows who is operating the register.
    /// </summary>
    public static class LoginService
    {
        private static string _currentUserName = "Okänd";

        public static string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                // Prevents null, empty or whitespace-only names
                if (string.IsNullOrWhiteSpace(value))
                    _currentUserName = "Okänd";
                else
                    _currentUserName = value.Trim();
            }
        }
    }
}
