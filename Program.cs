using System;
using System.Collections.Generic;
using BankApplication.Services;

using System.Linq;
namespace BankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            BankServices bankServices = new BankServices();
            AccountServices accountService = new AccountServices();
            TransactionServices transactionServices = new TransactionServices();
            UserServices userServices = new UserServices();
            List<Bank> banks = bankServices.GetBanks();
            Bank bank;
            while (true)
            {
               
                //  Name = "GlobalAdmin",
                // ID = "Global.Admin",
                // Password = "Gadmin@123",
            BankEntry:
                Console.WriteLine("1.Select bank\n2.Add bank\n");
                {
                    try
                    {
                        BankSelection Option = (BankSelection)Convert.ToInt32(Console.ReadLine());
                        switch (Option)
                        {
                            case BankSelection.SelectBank:
                                {
                                    bankEntry:
                                    Console.WriteLine("Select Bank\n");
                                    for (int index = 0; index < banks.Count; index++)
                                    {
                                        Console.WriteLine(index + 1 + "." + banks[index].Name);
                                    };
                                    try
                                    {
                                        int SelectedBank = Convert.ToInt32(Console.ReadLine());

                                        bank = banks[SelectedBank - 1];
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Select valid bank\n");
                                        goto bankEntry;
                                    }
                                    break;
                                }
                            case BankSelection.AddBank:
                                {
                                    Console.WriteLine("Enter GlobalAdmin UerID");
                                    string GAdminUserID = Console.ReadLine();
                                    Console.WriteLine("Enter password");
                                    string GAdminPassword = Console.ReadLine();

                                    if (userServices.IsGlobalAdmin(GAdminUserID, GAdminPassword))
                                    {
                                        Console.WriteLine("Enter bank name\n");
                                        string name = Console.ReadLine().ToUpper();
                                        Console.WriteLine("Enter Branch\n");
                                        string branch = Console.ReadLine().ToUpper();
                                        bank = bankServices.CreateBank(name, branch);
                                        Console.WriteLine("Add Admin\n");
                                        Console.WriteLine("Enter admin name\n");
                                        string AdminName = Console.ReadLine();
                                        User Admin = new User()
                                        {
                                            Name = AdminName,
                                            BankID = bank.ID,
                                            UserType = Role.admin
                                        };
                                        userServices.CreateUser(Admin);
                                        Console.WriteLine("Admin details:\nUserId:{0}\nPassword:{1}\n", Admin.ID, Admin.Password);
                                        goto BankEntry;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid login ID and password\n ");
                                        goto BankEntry;
                                    }
                                }
                            default:
                                {
                                    goto BankEntry;

                                }
                        }
                        while (true)
                        {
                            Console.WriteLine("Welcome to {0}", bank.Name);
                            Console.WriteLine("select an option\n1.Staff\n2.AccountHolder\n3.Admin\n4.Exit\n");
                            try
                            {
                                LoginOptions choice = (LoginOptions)Convert.ToInt32(Console.ReadLine());
                                switch (choice)
                                {
                                    case LoginOptions.staff:
                                        {
                                            Console.WriteLine("Staff login page\n");
                                            Console.WriteLine("Enter Staff UserID ");
                                            string userID = Console.ReadLine();
                                            Console.WriteLine("Enter Staff password");
                                            string password = Console.ReadLine();
                                            if (userServices.IsStaffMember(userID, password, bank.ID))
                                            {
                                                bool staffentry = true;
                                                while (staffentry)
                                                { 
                                                    Console.WriteLine("Choose a function\n1.Create an account\n2.Remove an account\n3.Revert Transaction\n4.Update account\n5.View Transaction history\n6.Add service charges for same bank transactions\n7.Add service charges for other bank transactions\n8.Add currency\n9.Remove currency\n10.log out\n");
                                                    try
                                                    {
                                                        StaffActions StaffFunction = (StaffActions)Convert.ToInt32(Console.ReadLine());
                                                        switch (StaffFunction)
                                                        {
                                                            case StaffActions.Create:
                                                                {
                                                                    Console.WriteLine("Enter user name");
                                                                    string name = Console.ReadLine();
                                                                    try
                                                                    {
                                                                        User custmer = new User() { Name = name, BankID = bank.ID };
                                                                        custmer=userServices.CreateUser(custmer);
                                                                        Account account = accountService.CreateAccount(custmer);
                                                                        Console.WriteLine("New Account details");
                                                                        Console.WriteLine("userID:{0}\npassword:{1}\naccountnumber:{2}\n", custmer.ID, custmer.Password, account.AccountNumber);
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.WriteLine("please give a valid name\n");
                                                                    }
                                                                    break;
                                                                }
                                                            case StaffActions.Remove:
                                                                {
                                                                    Console.WriteLine("Enter Account number");
                                                                    string Accountnumber = Console.ReadLine();

                                                                    if (accountService.IsValidAccount(Accountnumber,bank.ID))
                                                                    {
                                                                        if (accountService.RemoveAccount(Accountnumber))
                                                                        {
                                                                            Console.WriteLine("Account Removed ");
                                                                        }
                                                                        else
                                                                        {
                                                                            Console.WriteLine("Removing Action failed\n");
                                                                        }
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("This Account is not present in bank\n");
                                                                        break;
                                                                    }
                                                                }
                                                            case StaffActions.Revert:
                                                                {
                                                                    Console.WriteLine("Enter TransactionID ");
                                                                    string TransactionID = Console.ReadLine();
                                                                    Transaction transaction = transactionServices.GetTransaction(TransactionID);
                                                                    if (transactionServices.IsValidTransactionID(TransactionID))
                                                                    {
                                                                        
                                                                        Account toAccount = accountService.GetAccount(transaction.ToAccountNumber);
                                                                        if (transaction.Amount > toAccount.Balance)
                                                                        {
                                                                            Console.WriteLine("To bank account has less balance\n");
                                                                            break;
                                                                        }
                                                                        else
                                                                        {
                                                                            Account account = accountService.RevertTransaction(transaction);
                                                                            Transaction newtransaction = new Transaction()
                                                                            {
                                                                                FromAccountNumber = transaction.ToAccountNumber,
                                                                                ToAccountNumber = transaction.FromAccountNumber,
                                                                                Amount = transaction.Amount,
                                                                                ToBankID = transaction.BankID,
                                                                                BankID = transaction.ToBankID

                                                                            };
                                                                            transaction = transactionServices.CreateTransaction(newtransaction);
                                                                            Console.WriteLine("Transaction reverted successfully\n");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("Invalid TransactionID\n");
                                                                    }
                                                                    break;
                                                                }
                                                            case StaffActions.update:
                                                                {

                                                                    Console.WriteLine("Enter  UserID ");
                                                                    string userid = Console.ReadLine();
                                                                    Console.WriteLine("Enter old password");
                                                                    string Oldpassword = Console.ReadLine();
                                                                    if(userServices.IsCustemer(userid, Oldpassword, bank.ID))
                                                                    {
                                                                        Console.WriteLine("Enter new password");
                                                                        string newpassword = Console.ReadLine();
                                                                        User user = userServices.ResetPassword(userid, newpassword);
                                                                        Console.WriteLine("Updated successfully\n");
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("Invalid UserId&password\n");
                                                                    }

                                                                    break;
                                                                }
                                                            case StaffActions.TransactionHistory:
                                                                {
                                                                    Console.WriteLine("Enter Account number\n");
                                                                    string Accountnumber = Console.ReadLine();
                                                                    if (accountService.IsValidAccount(Accountnumber, bank.ID))
                                                                    {
                                                                        Account account = accountService.GetAccount(Accountnumber);
                                                                        List<Transaction> TransactionList = transactionServices.TransactionHistory(account.AccountNumber);
                                                                        Console.WriteLine("Number of Transactions:" + TransactionList.Count);
                                                                        foreach (var transaction in TransactionList)
                                                                        {
                                                                            Console.WriteLine("Amount:{0}\tFrom account:{1}\tToAccount:{2}\tTransactionID:{3}\t", transaction.Amount, transaction.FromAccountNumber, transaction.ToAccountNumber, transaction.ID);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("Invalid account number\n");
                                                                        break;
                                                                    }
                                                                    break;
                                                                }
                                                            case StaffActions.AddSameBankCharges:
                                                                {
                                                                    Console.WriteLine("Enter SameBankTransactionsRTGS:");
                                                                    int RTGS = Convert.ToInt32(Console.ReadLine());
                                                                    Console.WriteLine("Enter SameBankTransactionsIMPS:");
                                                                    int IMPS = Convert.ToInt32(Console.ReadLine());
                                                                   bank=bankServices.AddServiceChargesForSameBankTransactions(RTGS, IMPS, bank.ID);
                                                                    Console.WriteLine("Charges added successfully\n");
                                                                    break;
                                                                }
                                                            case StaffActions.AddOtherBankCharges:
                                                                {
                                                                    Console.WriteLine("Enter OtherBankTransactionsRTGS:");
                                                                    int RTGS = Convert.ToInt32(Console.ReadLine());
                                                                    Console.WriteLine("Enter OtherBankTransactionsRTGS:");
                                                                    int IMPS = Convert.ToInt32(Console.ReadLine());
                                                                  bank=bankServices.AddServiceChargesForOtherBankTransactions(RTGS, IMPS, bank.ID);
                                                                    Console.WriteLine("Charges added successfully\n");
                                                                    break;
                                                                }
                                                            case StaffActions.AddCurrency:
                                                                {
                                                                    Console.WriteLine("Enter Currency to Add");
                                                                    string currency = Console.ReadLine();
                                                                    Console.WriteLine("Enter Value of currency");
                                                                    decimal value = Convert.ToDecimal(Console.ReadLine());
                                                                   bank=bankServices.AddCurrency(currency, value, bank.ID);
                                                                    Console.WriteLine("Currency added successfully\n");
                                                                    break;
                                                                }
                                                            case StaffActions.RemoveCurrency:
                                                                {
                                                                    Console.WriteLine("Enter Currency to remove");
                                                                    string currency = Console.ReadLine();
                                                                    if (bank.Currencies.ContainsKey(currency))
                                                                    {
                                                                       bank=bankServices.RemoveCurrency(currency,bank.ID);
                                                                        Console.WriteLine("Currency removed successfully\n");
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("The Currency you entered is not present in bank\n ");
                                                                    }
                                                                    break;
                                                                }
                                                            case StaffActions.LogOut:
                                                                {
                                                                    Console.WriteLine("Thank you :)\n");
                                                                    staffentry = false;
                                                                    break;
                                                                }
                                                            default:
                                                                {
                                                                    Console.WriteLine("select a valid option\n");
                                                                    break;
                                                                }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Console.WriteLine("select a valid option\n");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Enter valid details\n");
                                            }
                                            break;
                                        }
                                    case LoginOptions.AccountHolder:
                                        {
                                            Console.WriteLine("AccountHolder login page\n");
                                            Console.WriteLine("Enter AccountHolder username");
                                            string userID = Console.ReadLine();
                                            Console.WriteLine("Enter AccountHolder password");

                                            string password = Console.ReadLine();
                                            if (userServices.IsCustemer(userID, password, bank.ID))
                                            {
                                                Account account = accountService.GetAccountWithUserID(userID);
                                                if (account == null)
                                                {
                                                    Console.WriteLine("you dont have any account in this bank\n");
                                                }
                                                bool coustmerentry = true;
                                                while (coustmerentry)
                                                {
                                                    Console.WriteLine("\n1.Deposite money\n2.Withdraw money\n3.Transfer money\n4.View Transaction history\n5.view balance\n6.log out\n");
                                                    try
                                                    {
                                                        CustmerOptions function = (CustmerOptions)Convert.ToInt32(Console.ReadLine());
                                                        switch (function)
                                                        {
                                                            case CustmerOptions.Deposit:
                                                                {
                                                                    Console.WriteLine("Enter Amount");
                                                                    decimal Amount = Convert.ToDecimal(Console.ReadLine());
                                                                    if (Amount > 0)
                                                                    {
                                                                        Console.WriteLine("list of Acceptable Currency");
                                                                        foreach (KeyValuePair<string, decimal> item in bank.Currencies)
                                                                        {
                                                                            Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                                                                        }
                                                                        Console.WriteLine("Enter currency key");
                                                                        string currency = Console.ReadLine().ToUpper();
                                                                        if (bank.Currencies.ContainsKey(currency))
                                                                        {
                                                                            Amount *= bank.Currencies[currency];
                                                                        }
                                                                        else
                                                                        {
                                                                            Console.WriteLine("Invalid Currency\n");
                                                                            break;
                                                                        }
                                                                        account = accountService.Deposit(account, Amount);
                                                                        Transaction transaction = new Transaction()
                                                                        {
                                                                            FromAccountNumber = account.AccountNumber,
                                                                            ToAccountNumber = account.AccountNumber,
                                                                            Amount = +Amount,
                                                                            ToBankID = bank.ID,
                                                                            BankID = bank.ID

                                                                        };
                                                                        Transaction newTransaction = transactionServices.CreateTransaction(transaction);
                                                                        Console.WriteLine("TransactionID:" + newTransaction.ID);
                                                                        Console.WriteLine("Transaction compleated\n");

                                                                        Console.WriteLine("Thank you :)");
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("Please enter valid amount\n");
                                                                        break;
                                                                    }
                                                                }
                                                            case CustmerOptions.WithDraw:
                                                                {
                                                                    Console.WriteLine("Enter Amount");
                                                                    decimal Amount = decimal.Parse(Console.ReadLine());
                                                                    if (Amount > account.Balance)
                                                                    {
                                                                        Console.WriteLine("Insufficient  balance\n");
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        account = accountService.Withdraw(account, Amount);
                                                                        Transaction transaction = new Transaction()
                                                                        {
                                                                            FromAccountNumber = account.AccountNumber,
                                                                            ToAccountNumber = account.AccountNumber,
                                                                            Amount = -Amount,
                                                                            ToBankID = bank.ID,
                                                                            BankID = bank.ID

                                                                        };
                                                                        transaction = transactionServices.CreateTransaction(transaction);
                                                                        Console.WriteLine("Transaction completed");
                                                                        Console.WriteLine("TransactionID:" + transaction.ID);
                                                                        Console.WriteLine("Thank you :)");
                                                                        break;
                                                                    }
                                                                }
                                                            case CustmerOptions.Transfer:
                                                                {
                                                                    Console.WriteLine("Enter Account number");
                                                                    string Accountnumber = Console.ReadLine();
                                                                TobankEntry:
                                                                    Bank ToBank;
                                                                    Console.WriteLine("Select Bank\n");
                                                                    for (int index = 0; index < banks.Count; index++)
                                                                    {
                                                                        Console.WriteLine(index + 1 + "." + banks[index].Name);
                                                                    };
                                                                    try
                                                                    {
                                                                        int SelectToBank = Convert.ToInt32(Console.ReadLine());
                                                                        ToBank = banks[SelectToBank - 1];
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.WriteLine("Select valid bank\n");
                                                                        goto TobankEntry;
                                                                    }
                                                                    if (accountService.IsValidAccount(Accountnumber, bank.ID))
                                                                    {
                                                                        Console.WriteLine("Enter Amount");
                                                                        decimal Amount = decimal.Parse(Console.ReadLine());

                                                                        if (Amount > account.Balance)
                                                                        {
                                                                            Console.WriteLine("Insufficient balance");
                                                                        }
                                                                        else
                                                                        {
                                                                            Account toAccount = accountService.GetAccount(Accountnumber);
                                                                            account = accountService.Transfer(account,toAccount, Amount);
                                                                            Transaction transaction = new Transaction()
                                                                            {
                                                                                FromAccountNumber = account.AccountNumber,
                                                                                ToAccountNumber = toAccount.AccountNumber,
                                                                                Amount = Amount,
                                                                                ToBankID = bank.ID,
                                                                                BankID = bank.ID

                                                                            };
                                                                            transaction = transactionServices.CreateTransaction(transaction);
                                                                            Console.WriteLine("TransactionID:" + transaction.ID);
                                                                            Console.WriteLine("Transaction compleated");
                                                                            Console.WriteLine("Thank you :)\n");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("This Account does not exist\n");
                                                                    }

                                                                    break;

                                                                }
                                                            case CustmerOptions.TransactionHistory:
                                                                {
                                                                    List<Transaction> TransactionList = transactionServices.TransactionHistory(account.AccountNumber);

                                                                    Console.WriteLine("Number of Transactions:" + TransactionList.Count);
                                                                    foreach (var transaction in TransactionList)
                                                                    {

                                                                        Console.WriteLine("Amount:{0}\tFrom account:{1}\tToAccount:{2}\tTransactionID:{3}\t", transaction.Amount, transaction.FromAccountNumber, transaction.ToAccountNumber, transaction.ID);
                                                                    }

                                                                    break;
                                                                }
                                                            case CustmerOptions.CheckBalance:
                                                                {
                                                                    Console.WriteLine("Balance:" + account.Balance);
                                                                    break;
                                                                }
                                                            case CustmerOptions.LogOut:
                                                                {
                                                                    Console.WriteLine("Thank you :)\n");
                                                                    coustmerentry = false;
                                                                    break;

                                                                }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Console.WriteLine("Invalid input\n");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("please Enter valid details\n");
                                            }
                                            break;
                                        }
                                    case LoginOptions.AddStaff:
                                        { Console.WriteLine("Admin login\n");
                                            Console.WriteLine("Enter Admin Name\n");
                                            string AdminName = Console.ReadLine();
                                            Console.WriteLine("Enter Admin passWord\n");
                                            string AdminPassword = Console.ReadLine();
                                            if (userServices.IsAdmin(AdminName, AdminPassword, bank.ID))
                                                {
                                                Console.WriteLine("Enter staff name\n ");
                                                string name = Console.ReadLine();
                                                User staff = new User()
                                                {
                                                    Name = name,
                                                    BankID = bank.ID,
                                                    UserType = Role.staff
                                                };
                                                userServices.CreateUser(staff);
                                                Console.WriteLine("staff details:\nUserId:{0}\nPassword:{1}", staff.ID, staff.Password);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid userId and Password\n");
                                                break;
                                            }
                                        }
                                    case LoginOptions.Exit:
                                        {
                                            Console.WriteLine("Thank you :)\n");
                                            goto BankEntry;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Invalid input\n");
                            }
                        }

                    }
                    catch
                    {
                        Console.WriteLine("Invalid input\n");
                    }
                }
            }
        }
    }
}

