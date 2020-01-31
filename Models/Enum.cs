using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication
{
    
        public enum Role { custmer, staff, admin, GlobalAdmin }
        public enum Charges
        {
            DefaultSameBankRTGS = 0,
            DefaultSameBankIMPS = 5,
            DefaultOtherBankRTGS = 2,
            DefaultOtherBankIMPS = 6,
        }
        public enum BankSelection
        {
            SelectBank = 1,
            AddBank = 2
        }
        public enum LoginOptions
        {
            staff = 1,
            AccountHolder = 2,
            AddStaff = 3,
            Exit = 4
        }
        public enum StaffActions
        {
            Create = 1,
            Remove = 2,
            Revert = 3,
            update = 4,
            TransactionHistory = 5,
            AddSameBankCharges = 6,
            AddOtherBankCharges = 7,
            AddCurrency = 8,
            RemoveCurrency = 9,
            LogOut = 10

        }
        public enum CustmerOptions
        {
            Deposit = 1,
            WithDraw = 2,
            Transfer = 3,
            TransactionHistory = 4,
            CheckBalance = 5,
            LogOut = 6
        }
        public enum Currency
        {
            INR =1
        }
       
    
}
