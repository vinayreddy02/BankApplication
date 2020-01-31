using System;
using System.Collections.Generic;
using System.Text;
using BankApplication;
using System.Linq;
using BankApplication.Services;

namespace BankApplication.Services
{
    class AccountServices
    {
        private List<Account> Accounts = new List<Account>();
        public List<Account> GetAccounts()
        {
            return Accounts;
        }
        public Account CreateAccount(User user)
        {
            try
            {
                Account account = new Account()
                {
                    UserID = user.ID,
                    AccountNumber = user.ID + DateTime.UtcNow.ToString("mmss"),
                    Balance = 0,
                    BankID = user.BankID
                };

                Accounts.Add(account);
                return account;
            }
            catch
            {
                return null;
            }

        }
        public Account GetAccount(string accountNumber)
        {
            try
            {
                return Accounts.First(account => string.Equals(account.AccountNumber, accountNumber));
            }
            catch
            {
                return null;
            }
        }
        public Account GetAccountWithUserID(string ID)
        {
            try
            {
                return Accounts.First(account => string.Equals(account.UserID, ID));
                
            }
            catch
            {
                return null;
            }
        }
        public bool RemoveAccount(string ID)
        {
            try
            {
               
               Accounts.Where(account => account.AccountNumber != ID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsValidAccount(string accountNumber, string bankID)
        {
            return Accounts.Any(account => string.Equals(account.AccountNumber, accountNumber) && string.Equals(account.BankID, bankID));
        }
        public Account Withdraw(Account account, decimal amount)
        {
            try
            {
                account.Balance -= amount;
                return account;
            }
            catch
            {
                return null;
            }
        }
        public Account Deposit(Account account, decimal amount)
        {
            try
            {
                account.Balance += amount;
                return account;
            }
            catch
            {
                return null;
            }
        }
        public Account Transfer(Account account, Account reciverAccount, decimal amount)
        {
            try
            {
                BankServices bankServices = new BankServices();
                Bank bank = bankServices.GetBank(account.BankID);
                account.Balance -= amount;
                reciverAccount.Balance += amount;
                if (account.BankID.Equals(reciverAccount.BankID))
                {
                    account.Balance -= (amount * (bank.sameBankRTGS + bank.sameBankIMPS) * (decimal)0.01);
                }
                else
                {
                    account.Balance -= (amount * (bank.otherBankRTGS + bank.otherBankIMPS) * (decimal)0.01);
                }
                return account;
            }
            catch
            {
                return null;
            }
        }
        public Account RevertTransaction(Transaction transaction)
        {
            try
            {
                Account account = GetAccount(transaction.FromAccountNumber);
                Account toAccount = GetAccount(transaction.ToAccountNumber);
                account.Balance += transaction.Amount;
                toAccount.Balance -= transaction.Amount;
                return account;
            }
            catch
            {
                return null;
            }
           
        }
    }
}



