using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string ID { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToBankID { get; set; }
        public string BankID { get; set; }

    }
}
