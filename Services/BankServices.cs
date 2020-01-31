using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Models;
using System.Linq;


namespace BankApplication.Services
{
    class BankServices
    {

        private List<Bank> Banks = UtilServices.InitiateBanks();
        public List<Bank> GetBanks()
        {

            return Banks;
        }

        public Bank CreateBank(string name,string branch)
        {
            try
            {
                Bank bank = new Bank()
                {
                    Name = name,
                    ID = name + DateTime.UtcNow.ToString("mmss"),
                    Branch = branch,
                    IFSCcode = Constants.IFSCPrefix + DateTime.UtcNow.ToString("mmss")
                };
                Banks.Add(bank);
                return bank;
            }
            catch
            {
                return null;
            }
        }
        public Bank GetBank(string ID)
        {
    return Banks.FirstOrDefault(bank => string.Equals(bank.ID, ID));
          
        }

        public bool RemoveBank(string ID)
        {
           
            try
            {
               Banks.Where(bank=>bank.ID!=ID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Bank AddServiceChargesForSameBankTransactions(int rtgs,int imps,string bankid)
        {
            Bank bank = GetBank(bankid);
            try
            {
                bank.sameBankRTGS = rtgs;
                bank.sameBankRTGS = imps;
                return bank;
            }
            catch
            {
                return null;
            }
        }
        public Bank AddServiceChargesForOtherBankTransactions(int rtgs, int imps,string bankid)
        {
            Bank bank = GetBank(bankid);
            try
            {
                bank.otherBankRTGS = rtgs;
                bank.otherBankIMPS = imps;
                return bank;
            }
            catch
            {
                return null;
            }
        }
        public Bank AddCurrency(string currency,decimal value,string bankid)
        {
            Bank bank = GetBank(bankid);
            try
            {
                bank.Currencies.Add(currency, value);
                return bank;
            }
            catch
            {
                return null;
            }

        }
        public Bank RemoveCurrency(string currency, string bankid)
        {
            Bank bank = GetBank(bankid);
            try
            {
                bank.Currencies.Remove(currency);
                return bank;
            }
            catch
            {
                return null;
            }

        }
    }
}
