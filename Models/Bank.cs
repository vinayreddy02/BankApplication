using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication
{
    public class Bank
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Branch { get; set; }
        public string IFSCcode { get; set; }
        public decimal sameBankRTGS =Convert.ToDecimal(Charges.DefaultSameBankRTGS);
        public decimal sameBankIMPS = Convert.ToDecimal(Charges.DefaultSameBankIMPS);
        public decimal otherBankRTGS = Convert.ToDecimal(Charges.DefaultOtherBankRTGS);
        public decimal otherBankIMPS = Convert.ToDecimal(Charges.DefaultOtherBankIMPS);
        public Dictionary<string, decimal> Currencies = new Dictionary<string, decimal>()
            { {Convert.ToString(Currency.INR),Convert.ToDecimal(Currency.INR) } };

    }
}
