using System;
using System.Collections.Generic;
using System.Text;
using PL;
using AdminBLL;
using LoginCredentialsBO;
using CustomerAccountBO;

namespace AdminPL
{
    public class ATMAdminView:ATMView
    {
        internal static void AdminMenu()
        {
            Console.WriteLine("----Logged in as Admin----\n");
            Console.WriteLine("1----Create New Account");
            Console.WriteLine("2----Delete Existing Account");
            Console.WriteLine("3----Update Account Information");
            Console.WriteLine("4----Search for Account");
            Console.WriteLine("5----View Reports");
            Console.WriteLine("6----Exit");
        }
        public static void MenuChoice()
        {
            string input = string.Empty;
            AdminMenu();
            do
            {
                Console.Write("\nPlease select one of the above options: ");
                input = Console.ReadLine();
                if (input == "1")
                {
                    CreateNewAccount();
                }
                else if (input == "2")
                {
                    //DeleteExistingAccount();
                }
                else if (input == "3")
                {
                    //UpdateAccountInformation();
                }
                else if (input == "4")
                {
                    //SearchForAccounts();
                }
                else if (input == "5")
                {
                    //ViewReports();
                }
                else if (input != "6")
                {
                    Console.WriteLine("Invalid Input!\n");
                }

            } while (input != "6");
        }
        public static void CreateNewAccount()
        {
            Console.WriteLine("---Create New Account----\n");
            bool alreadyExist = false;
            string login = string.Empty, pinCode = string.Empty, name = string.Empty;
            string type = string.Empty, bal = string.Empty, status = string.Empty;
            do
            {
                //---------------Login input------------
                Console.Write("Login: ");
                login = Console.ReadLine();
                while (login == string.Empty)
                {
                    Console.WriteLine("Invalid Login!\n");
                    Console.Write("Login: ");
                    login = Console.ReadLine();
                }

                //--------------pin code input------------
                Console.Write("Pin Code (5-digit): ");
                pinCode = Console.ReadLine();
                bool rightPin = ValidatePinCode(pinCode);
                while (rightPin == false)
                {
                    Console.WriteLine("\nInvalid Pincode!");
                    Console.Write("Pin Code (5-digit): ");
                    pinCode = Console.ReadLine();

                    rightPin = ValidatePinCode(pinCode);
                }

                //-------------Holders name input---------
                Console.Write("Holders Name: ");
                name = Console.ReadLine();
                while (name == string.Empty)
                {
                    Console.WriteLine("Invalid Name!\n");
                    Console.Write("Holders Name: ");
                    name = Console.ReadLine();
                }

                //-------------type input----------
                Console.Write("Type (Savings/Current): ");
                type = Console.ReadLine();
                while (!(type.ToLower() == "savings" || type.ToLower() == "current"))
                {
                    Console.WriteLine("\nInvalid Type!");
                    Console.Write("Type (Savings/Current): ");
                    type = Console.ReadLine();
                }

                //----------------starting balance input-------
                Console.Write("Starting Balance: ");
                bal = Console.ReadLine();
                bool rightBalance = ValidateBalance(bal);
                while (rightBalance == false)
                {
                    Console.WriteLine("\nInvalid Balance!");
                    Console.Write("Starting Balance: ");
                    bal = Console.ReadLine();
                    bal = bal.Replace(",", "");     //if balance is of type 80,000

                    rightBalance = ValidateBalance(bal);
                }

                //----------------status input--------------
                Console.Write("Status (Active/Blocked): ");
                status = Console.ReadLine();
                while (!(status.ToLower() == "active" || status.ToLower() == "blocked"))
                {
                    Console.WriteLine("\nInvalid Input!");
                    Console.Write("Status: ");
                    status = Console.ReadLine();
                }

                LoginCredentials cred = new LoginCredentials(); //login BO to save in login table
                cred.Login = login;
                cred.PinCode = int.Parse(pinCode);

                alreadyExist = ATMAdminManager.IsUserExist(cred);
                if (alreadyExist)
                {
                    Console.WriteLine("The 'Login' you entered already exists. Try a different one");
                }
            } while (alreadyExist == true);
            
            CustomerAccount account = new CustomerAccount(); //creating new account BO
            account.HoldersName = name;
            account.Type = type;
            account.Balance = int.Parse(bal);
            account.Status = status;

            LoginCredentials cred2 = new LoginCredentials();
            cred2.Login = login;
            cred2.PinCode = int.Parse(pinCode);

            if (ATMAdminManager.CreateLogin(cred2))  //create login for this account first
            {
                int accID = ATMAdminManager.CreateAccount(account, login);   //pass data to BLL
                Console.WriteLine("Account Successfully Created – the account number assigned is: " + accID);
            }
            else
            {
                Console.WriteLine("couldn't create account");
            }
        }
       /*
        public static void DeleteExistingAccount()
        {
            Console.Write("Enter the account number which you want to delete: ");
            int accNum = int.Parse(Console.ReadLine());

            ATMAdminManager mng = new ATMAdminManager();
            string holdersName = mng.GetAccountHoldersName(accNum);
            if (holdersName == "none")
            {
                Console.WriteLine("\nAccount doesn't Exist");
            }
            else
            {
                Console.Write($"\nYou wish to delete the account held by {holdersName}; If this information is correct please re - enter the account number: ");
                int accNum2 = int.Parse(Console.ReadLine());
                if (accNum == accNum2)
                {
                    ATMAdminDAL.
                }
                else
                {
                    while (accNum2 != accNum)
                    {
                        Console.Write("Please re-enter same Account Number: ");
                        accNum2 = int.Parse(Console.ReadLine());
                    }
                }
            }

        }
        public static void UpdateAccountInformation()
        {
            Console.Write("Enter Account Number: ");
            int accNum = int.Parse(Console.ReadLine());

            ATMAdminManager mng = new ATMAdminManager();
            CustomerAccount OldInfo = mng.getAccount(accNum);
            if (OldInfo == null)
            {
                Console.WriteLine("Account not exist!\n");
            }
            else
            {
                DisplayOldInformation(OldInfo);
                Console.WriteLine("Please enter in the fields you wish to update (leave blank otherwise):\n");
                Console.Write("Login: ");
                string login = Console.ReadLine();
                Console.Write("PinCode: ");
                string pincode = Console.ReadLine();
                Console.Write("Holders Name: ");
                string name = Console.ReadLine();
                Console.Write("Status: ");
                string status = Console.ReadLine();

                CustomerAccount updated = new CustomerAccount();
                updated.AccountID = OldInfo.AccountID;
                if (login == string.Empty)
                {

                }
                if (name == string.Empty)
                {
                    updated.HoldersName = OldInfo.HoldersName;
                }


            }

        }
        internal static void DisplayOldInformation(CustomerAccount account)
        {
            Console.WriteLine("Account # " + account.AccountID);
            Console.WriteLine("Type: " + account.Type);
            Console.WriteLine("Holder: " + account.HoldersName);
            Console.WriteLine("Balance: " + account.Balance);
            Console.WriteLine("Status: " + account.Status);
        }
        public static void SearchForAccounts()
        {

        }
        public static void ViewReports()
        {

        }*/

        internal static bool ValidateBalance(string bal)
        {
            //bal = bal.Replace(",", "");     //if balance is of type 80,000
            if (int.TryParse(bal, out int b))
            {
                //Console.WriteLine("convertable");
                if (int.Parse(bal)>0)
                {
                    //acceptable
                    Console.WriteLine("accepted");
                    return true;
                }
                else
                {
                    //Console.WriteLine("");
                    return false;
                }
            }
            return false;               //if not valid 
        }
    }
}
