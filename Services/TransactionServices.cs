using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Services;
using BankApplication.Models;
using System.Linq;

namespace BankApplication.Services
{
    class TransactionServices
    {
        private List<Transaction> Transactions = new List<Transaction>();
        public List<Transaction> GetAllTransactions()
        {
            return Transactions;
        }
        public Transaction CreateTransaction(Transaction transaction)
        {
            try
            {
                Transaction newTransaction = new Transaction()
                {
                    ID = Convert.ToString(Constants.TransactionIDPrefix) + transaction.BankID + transaction.FromAccountNumber + DateTime.UtcNow.ToString("yyyyMMddss"),
                    Amount = transaction.Amount,
                    FromAccountNumber = transaction.FromAccountNumber,
                    ToAccountNumber = transaction.ToAccountNumber,
                    ToBankID = transaction.ToBankID
                };
                Transactions.Add(newTransaction);
                return newTransaction;
            }
            catch
            {
                return null;
            }
        }
        public Transaction GetTransaction(string ID)
        {
            try
            {
                return Transactions.FirstOrDefault(transaction => string.Equals(transaction.ID, ID));
            }
            catch
            {
                return null;
            }
        }
        public bool RemoveTransaction(string ID)
        {
            try
            {
                Transactions.Where(transaction => transaction.ID != ID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Transaction> TransactionHistory(string AccountNumber)
        {
            try
            {
                return Transactions.FindAll(transaction => string.Equals(transaction.FromAccountNumber, AccountNumber) || string.Equals(transaction.ToAccountNumber, AccountNumber));
            }
            catch
            {
                return null;
            }
        }
        public bool IsValidTransactionID(string transactionID)
        {
            return Transactions.Any(transaction => string.Equals(transaction.ID, transactionID));
        }
    }
}
