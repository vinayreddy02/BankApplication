using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.Services
{
    class UtilServices
    {
        public static List<User> InitiateStaff()
        {
            List<User> staff = new List<User>()
            {
            new User()
            {
                Name = "saharsh",
                ID = "saharsh.s",
                Password = "sah@123",
                UserType = Role.staff,
                BankID="HDF2056"
            },     
            new User()
            {
                Name = "vinay",
                ID = "vinay.s",
                Password = "vin@123",
                UserType = Role.staff,
                BankID="SBI2043"
            },
            new User()
            {
                Name="Admin",
                ID="HDFAdmin.s",
                Password="admin@123",
                UserType=Role.admin,
                BankID="SBI2043"
            },
             new User()
            {
                Name="Admin",
                ID="SBIAdmin.s",
                Password="admin@123",
                UserType=Role.admin,
                BankID="HDF2056"
             },
             new User()
             {
                 Name="GlobalAdmin",
                 ID="Global.Admin",
                 Password="Gadmin@123",
                 UserType=Role.GlobalAdmin
             }
            };
            return staff;
        }

        public static List<Bank> InitiateBanks()
        {
            List<Bank> Banks = new List<Bank>()
            {
            new Bank()
            {
                Name = "HDFC",
                ID = "HDF2056",
                Branch = "Hydrabad",
                IFSCcode = "IN2057",
            },

            new Bank()
            {
                Name = "SBI",
                ID = "SBI2043",
                Branch = "Hydrabad",
                IFSCcode = "IN2066",
            }
            };

            return Banks;
        }
    }
}
