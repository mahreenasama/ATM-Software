using System;
using System.Collections.Generic;
using System.Text;
using CustomerAccountBO;
using LoginCredentialsBO;
using BLL;
using System.Linq;
using TransactionsBO;

namespace PL
{
    public class AdminView : ATMView
    {
        internal static void AdminMenu()
        {
            /*
             * function displays Menu choices of Admin
             * arguments: no
             * return: nothing
            */
            Console.WriteLine("\n--------Logged in as Admin-------\n");
            Console.WriteLine("1----Create New Account");
            Console.WriteLine("2----Delete Existing Account");
            Console.WriteLine("3----Update Account Information");
            Console.WriteLine("4----Search for Account");
            Console.WriteLine("5----View Reports");
            Console.WriteLine("6----Exit");
        }
        public static void MenuChoice()
        {
            /*
             * function takes input of Admin's menu choice and calls respective functions
             * arguments: no
             * return: nothing
            */
            string input = string.Empty;
            do
            {
                AdminMenu();
                Console.Write("\nPlease select one of the above options: ");
                input = Console.ReadLine();
                if (input == "1")
                {
                    CreateNewAccount();             //call for respective functions
                }
                else if (input == "2")
                {
                    DeleteExistingAccount();
                }
                else if (input == "3")
                {
                    UpdateAccountInformation();
                }
                else if (input == "4")
                {
                    SearchForAccounts();
                }
                else if (input == "5")
                {
                    ViewReports();
                }
                else if (input != "6")
                {
                    Console.WriteLine("Invalid Input!\n");
                }

            } while (input != "6");
        }
        public static void CreateNewAccount()
        {
            /*
             * function takes all inputs fro creating new Account, validates them and passes to BLL(to create account)
             * arguments: no
             * return: nothing
            */
            bool alreadyExist = false;
            string login = string.Empty, pinCode = string.Empty, name = string.Empty;
            string type = string.Empty, bal = string.Empty, status = string.Empty;

            Console.WriteLine("\n--------Create New Account--------\n");

            Console.Write("Enter Login: ");                //login
            login = Console.ReadLine();
            login = CheckString(login, "Enter Login");     //base class method

            Console.Write("Enter pin code (5-digit): ");   //pincode
            pinCode = Console.ReadLine();
            pinCode = CheckPinCode(pinCode);                //base class method

            Console.Write("Holders Name: ");                //holders name
            name = Console.ReadLine();
            name = CheckString(name, "Holders Name");       //base class method

            Console.Write("Type (Savings, Current): ");      //type
            type = Console.ReadLine();
            type = CheckType(type);

            Console.Write("Starting Balance: ");             //starting balance
            bal = Console.ReadLine();
            bal = CheckBalance(bal, "Starting Balance");     //base class method

            Console.Write("Status (Active, Disabled): ");    //status
            status = Console.ReadLine();
            status = CheckStatus(status);


            LoginCredentials cred = new LoginCredentials();  //login BO
            cred.Login = login.ToLower();
            cred.PinCode = pinCode;

            alreadyExist = AdminManager.IsUserExist(cred);   //check if this username(login) already exists
            if (alreadyExist)
            {
                Console.WriteLine("\nThe 'Login' you entered already exists. Try a different one.");
            }
            else
            {
                CustomerAccount account = new CustomerAccount();      //creating new account BO
                account.HoldersName = name;
                account.Type = type;
                account.Balance = int.Parse(bal);
                account.Status = status;

                LoginCredentials cred2 = new LoginCredentials();       //login for new account
                cred2.Login = login.ToLower();
                cred2.PinCode = pinCode;

                int accID = AdminManager.CreateAccount(account, cred2);     //pass data to BLL
                if (accID > 0)
                    Console.WriteLine("\nAccount Successfully Created – the account number assigned is: " + accID);
                else
                    Console.WriteLine("\ncouldn't create account");
            }
        }

        //---------------------------Delete existing account------------
        public static void DeleteExistingAccount()
        {
            /*
             * function takes input of account number and passes to BLL (to delete that account)
             * arguments: no
             * return: nothing
            */
            Console.Write("\nEnter the account number which you want to delete: ");
            string accNumStr = Console.ReadLine();
            accNumStr = CheckID(accNumStr);                 //base class method
            int accNum = int.Parse(accNumStr);              //parsing

            //---pass to BLL
            CustomerAccount acc = ATMManager.GetAccount(accNum);  //to get account holders name
            if (acc == null)
            {
                Console.WriteLine("No such account exist");
            }
            else
            {
                string holderName = acc.HoldersName.Trim();
                Console.Write($"\nYou wish to delete the account held by {holderName}; If this information is correct please re - enter the account number: ");
                string accNum2str = Console.ReadLine();
                while (accNum2str != accNumStr)
                {
                    Console.Write("\nPlease re-enter same Account Number: ");
                    accNum2str = Console.ReadLine();
                }
                if (AdminManager.DeleteAccount(accNum) > 0)             //delete when both same
                    Console.WriteLine("\nAccount deleted successfully.");
                else
                    Console.WriteLine("couldn't delete account\n");
            }
        }

        //-------------------------------update account-----------------
        public static void UpdateAccountInformation()
        {
            /*
             * function takes all inputs to update an existing account, and passes to BLL
             * arguments: no
             * return: nothing
            */
            Console.Write("\nEnter Account Number: ");
            string accNumStr = Console.ReadLine();
            accNumStr = CheckID(accNumStr);                //base class method
            int accNum = int.Parse(accNumStr);             //parsing 

            CustomerAccount OldInfo = ATMManager.GetAccount(accNum);        //getting old info
            LoginCredentials OldCred = AdminManager.GetCredentials(accNum);

            if (OldInfo == null)
            {
                Console.WriteLine("No such account exist!\n");
            }
            else
            {
                DisplayOldInformation(OldInfo, OldCred);    //internal function
                bool alreadyExist = false;
                string login = string.Empty, pincode = string.Empty, name = string.Empty;
                string status = string.Empty;

                Console.WriteLine("\nPlease enter in the fields you wish to update (leave blank otherwise):\n");

                Console.Write("Login: ");
                login = Console.ReadLine();
                if (login == string.Empty)                //if left blank
                    login = OldCred.Login;

                Console.Write("PinCode: ");
                pincode = Console.ReadLine();
                if (pincode == string.Empty)              //if left blank
                    pincode = OldCred.PinCode;
                else
                    pincode = CheckPinCode(pincode);      //base class method

                Console.Write("Holders Name: ");
                name = Console.ReadLine();
                if (name == string.Empty)
                    name = OldInfo.HoldersName;            //if left blank

                Console.Write("Status (Active, Disabled): ");
                status = Console.ReadLine();
                if (status == string.Empty)
                    status = OldInfo.Status;
                else
                    status = CheckStatus(status);           //internal function

                //----
                LoginCredentials cred = new LoginCredentials(); //login BO
                cred.Login = login.ToLower();
                cred.PinCode = pincode;

                if (login.ToLower() != OldCred.Login.ToLower())
                    alreadyExist = AdminManager.IsUserExist(cred);  //check if this username(login) already exists
                if (alreadyExist)
                {
                    Console.WriteLine("\nThe 'Login' you entered already exists. Try a different one");
                }
                else
                {
                    LoginCredentials cred2 = new LoginCredentials();    //creating new credentials
                    cred2.Login = login;
                    cred2.PinCode = pincode;

                    CustomerAccount updateAcc = new CustomerAccount();  //new account(updated)
                    updateAcc.HoldersName = name;
                    updateAcc.Status = status;

                    //---pass to BLL
                    if (AdminManager.UpdateAccount(accNum, updateAcc, cred2) > 0)
                        Console.WriteLine("\nAccount updates successfully\n");
                    else
                        Console.WriteLine("couldn't update account");
                }
            }
        }
        internal static void DisplayOldInformation(CustomerAccount account, LoginCredentials cred)
        {
            /*
             * function displays account details of received account
             * arguments: Account (object, to display its details)
             * return: nothing
            */
            Console.WriteLine("\nAccount # " + account.Id);
            Console.WriteLine("Login: " + cred.Login);
            Console.WriteLine("PinCode: " + cred.PinCode);
            Console.WriteLine("Type: " + account.Type);
            Console.WriteLine("Holder: " + account.HoldersName);
            Console.WriteLine("Balance: " + account.Balance);
            Console.WriteLine("Status: " + account.Status);
        }

        //--------------------------search for accounts----------------------
        public static void SearchForAccounts()
        {
            /*
             * function takes inputs for search criteria (to search for particular accounts in DB), and displays search result obtained 
             * arguments: no
             * return: nothing
            */
            Console.WriteLine("\nSEARCH MENU:\n");

            Console.Write("Account ID: ");                      //account number
            string accNumStr = Console.ReadLine();
            if (accNumStr != string.Empty)
                accNumStr = CheckID(accNumStr);
            
            Console.Write("User ID: ");                         //username
            string userID = Console.ReadLine();
            
            Console.Write("Holders Name: ");                    //account holder name
            string name = Console.ReadLine();

            Console.Write("Type (Savings, Current): ");         //account type
            string type = Console.ReadLine();
            if (type != string.Empty)
                type = CheckType(type);

            Console.Write("Balance: ");                         //balance
            string bal = Console.ReadLine();
            if (bal != string.Empty)
                bal = CheckBalance(bal);

            Console.Write("Status: ");                          //account status
            string status = Console.ReadLine();
            if (status != string.Empty)
                status = CheckStatus(status);

            //----------
            List<string> searchCrieteria = new List<string>();
            searchCrieteria.Add(accNumStr);                     //preparing search list
            searchCrieteria.Add(userID);
            searchCrieteria.Add(name);
            searchCrieteria.Add(type);
            searchCrieteria.Add(bal);
            searchCrieteria.Add(status);

            var data = AdminManager.SearchAccounts(searchCrieteria);    //pass to BLL
            List<CustomerAccount> accts = data.Item1;
            List<LoginCredentials> creds = data.Item2;

            DisplaySearchedResult(accts, creds);                //internal function
        }
        internal static void DisplaySearchedResult(List<CustomerAccount> accts, List<LoginCredentials> creds)
        {
            /*
             * function displays accounts & their credentials details received
             * arguments: Accounts & credentials details
             * return: nothing
            */
            Console.Write(
                format: "\n{0,-15} {1,-15} {2,-15}",
                arg0: "Account ID",
                arg1: "User ID",
                arg2: "PinCode"
                );
            Console.Write(
                format: "{0,-20} {1,-20} {2,-20}",
                arg0: "Holders Name",
                arg1: "Type",
                arg2: "Balance"
                );
            Console.Write(
                format: "{0,-10}\n",
                arg0: "Status"
                );
            for (int i = 0; i < accts.Count; i++)
            {
                Console.Write(
              format: "{0,-15} {1,-15} {2,-15}",
              arg0: accts[i].Id,
              arg1: creds[i].Login,
              arg2: creds[i].PinCode
              );
                Console.Write(
                    format: "{0,-20} {1,-20} {2,-20:N0}",
                    arg0: accts[i].HoldersName.Trim(),
                    arg1: accts[i].Type,
                    arg2: accts[i].Balance
                    );
                Console.Write(
               format: "{0,-10}\n",
               arg0: accts[i].Status
               );
            }
        }
        //-----------------------------view reports--------------------------
        public static void ViewReports()
        {
            /*
             * function takes input choice to view reports, and calls respective functions
             * arguments: no
             * return: nothing
            */
            string input = string.Empty;
            do
            {
                Console.WriteLine("1----Accounts By Amount");
                Console.WriteLine("2----Accounts By Date");
                Console.Write("\nEnter your choice: ");
                input = Console.ReadLine();
                if (input == "1")
                {
                    ViewByAmount();                 //call for respective function (internal)
                }
                else if (input == "2")
                {
                    ViewByDate();
                }
                else
                {
                    Console.WriteLine("Invalid!\n");
                }
            } while (!(input == "1" || input == "2"));
        }
        internal static void ViewByAmount()
        {
            /*
             * function takes max,min balance inputs and displays Reports result (obtained from BLL)
             * arguments: no
             * return: nothing
            */
            string min = string.Empty, max = string.Empty;
            do
            {
                Console.Write("Enter the minimum amount: ");
                min = Console.ReadLine();
                min = CheckBalance(min);                        //input validation
                Console.Write("Enter the maximum amount: ");
                max = Console.ReadLine();
                max = CheckBalance(max);

                if (int.Parse(min) > int.Parse(max))            //if max < min
                {
                    Console.WriteLine("Invalid Range!\n");
                }
            } while (int.Parse(min) > int.Parse(max));
                                                                //pass to BLL
            var data = AdminManager.ViewReportsByAmount(int.Parse(min), int.Parse(max));
            List<CustomerAccount> accts = data.Item1;
            List<LoginCredentials> creds = data.Item2;

            DisplaySearchedResult(accts, creds);                //internal function
        }
        internal static void ViewByDate()
        {
            /*
             * function takes account number,start,end date inputs and displays Reports result (obtained from BLL)
             * arguments: no
             * return: nothing
            */
            string start = string.Empty, end = string.Empty, accNumStr = string.Empty;

            Console.Write("\nEnter Account number: ");
            accNumStr = Console.ReadLine();
            accNumStr = CheckID(accNumStr);
            Console.Write("Enter the starting date (dd/mm/yyyy): ");
            start = Console.ReadLine();
            start = DateCheck(start, " starting");                   //input validation
            Console.Write("Enter the ending date (dd/mm/yyyy): ");
            end = Console.ReadLine();
            end = DateCheck(end, " ending");

            //---pass data to BLL 
            var data = AdminManager.ViewReportsByDate(start, end, int.Parse(accNumStr));
            List<Transaction> trans = data.Item1;
            List<LoginCredentials> creds = data.Item2;
            List<string> accHolders = data.Item3;

            DisplayViewByDateAccounts(trans, creds, accHolders);        //internal function
        }
        internal static void DisplayViewByDateAccounts(List<Transaction> trans,List<LoginCredentials> creds, List<string> accHoldres)
        {
            /*
             * function displays Amount transaction reports and their respective credentials
             * arguments: transaction reports & credentials
             * return: nothing
            */
            Console.Write(
                format: "\n{0,-10} {1,-15} {2,-15}",
                arg0: "Transaction Type",
                arg1: "User ID",
                arg2: "PinCode"
                );
            Console.Write(
                format: "{0,-10} {1,-20} {2,-20}\n",
                arg0: "Holders Name",
                arg1: "Amount",
                arg2: "Date"
                );
            for (int i = 0; i < trans.Count; i++)
            {
                Console.Write(
              format: "{0,-10} {1,-15} {2,-15}",
              arg0: trans[i].Type,
              arg1: creds[i].Login,
              arg2: creds[i].PinCode
              );
                Console.Write(
                    format: "{0,-10} {1,-20:N0} {2,-20}\n",
                    arg0: accHoldres[i].Trim(),
                    arg1: trans[i].Amount,
                    arg2: trans[i].Date
                    );
            }
        }
        
        //------------------helping fuctions---------
        internal static string DateCheck(string date,string se)
        {
            /*
             * function checks validity of date (format)
             * arguments: date (string), a helping string 'se' to indicate 'start/end date'
             * return: date (string, valid)
            */
            while (true)
            {
                string[] dates = date.Split("/");
                int day = 0, month = 0, year = 0;
                if (uint.TryParse(dates[0], out uint d) && uint.TryParse(dates[1], out uint m) && uint.TryParse(dates[2], out uint y))
                {
                    day = int.Parse(dates[0]);
                    month = int.Parse(dates[1]);
                    year = int.Parse(dates[2]);
                    if (day > 0 && day <= 31 && month > 0 && month <= 12)
                        break;
                }
                Console.WriteLine("Invalid Date!\n");
                Console.Write("Enter the" + se + " date (dd/mm/yyyy): ");
                date = Console.ReadLine();
            }
            return date;
        }
        internal static string CheckType(string type)
        {
            /*
             * function checks validity of account type (savings / current)
             * arguments: type (to be validated)
             * return: account type (valid)
            */
            while (!(type.ToLower() == "savings" || type.ToLower() == "current"))
            {
                Console.WriteLine("Invalid Type!\n");
                Console.Write("Type (Savings, Current): ");
                type = Console.ReadLine();
            }
            return type;
        }
        internal static string CheckStatus(string status)
        {
            /*
             * function checks validity of account status (active / disabled)
             * arguments: status (to be validated)
             * return: account status (valid)
            */
            while (!(status.ToLower() == "active" || status.ToLower() == "disabled"))
            {
                Console.WriteLine("Invalid!\n");
                Console.Write("Status (Active, Disabled): ");
                status = Console.ReadLine();
            }
            return status;
        }
    }
}
