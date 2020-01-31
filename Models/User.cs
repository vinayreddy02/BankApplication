using System;
using System.Collections.Generic;
using System.Text;


namespace BankApplication
{
    public class User
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Password { get; set; }
        public string BankID { get; set; }
        public Role UserType { get; set; }
    }
}
