using System;

namespace CashRegisterSystem.Data.Service
{
    /// <summary>
    /// Stores the currently active cash register number.
    /// Used so the system always knows which register is in use.
    /// </summary>
    public static class RegisterService
    {
        // The active register number. Default is 1.
        private static int _currentRegister = 1;

        public static int CurrentRegister
        {
            get => _currentRegister;
            set
            {
                // Prevents invalid register numbers (0 or negative)
                if (value <= 0)
                    _currentRegister = 1;
                else
                    _currentRegister = value;
            }
        }
    }
}
