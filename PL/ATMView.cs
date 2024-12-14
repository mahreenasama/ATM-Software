using System;
using System.Collections.Generic;
using System.Text;
using BLL;
using LoginCredentialsBO;
using CustomerAccountBO;
using System.Linq;

namespace PL
{
    public class ATMView                                        //base class
    {
        public static void InputLoginInfo()
        {
            /*
             * function takes input of Login credentials and presents user Menu accordingly 
             * arguments: no
             * return: nothing
            */
            Console.Write("Enter Login: ");
            string login = Console.ReadLine();
            login = CheckString(login, "Enter Login");         //taking inputs
            
            Console.Write("Enter pin code (5-digit): ");
            string pincode = Console.ReadLine();
            pincode = CheckPinCode(pincode);                  

            LoginCredentials cred = new LoginCredentials();     //creating BO
            cred.Login = login.ToLower();
            cred.PinCode = pincode;

            string role = ATMManager.CheckLoginBLL(cred);       //pass data to BLL

            DisplayMenu(role, cred, login);                     //menu display accrding to role of user
        }

        //---------------------helping functions-------------------
        internal static void DisplayMenu(string role, LoginCredentials cred, string login)
        {
            /*
             * function displays menu accrding to user Role(admin,customer) OR error messages (if user not exists)
             * arguments: role & credentials (of current user)
             * return: nothing
            */
            bool stop = false;
            while (stop == false)
            {
                if (role == "admin")
                {
                    stop = true;
                    AdminView.MenuChoice();                         //display menu accordingly
                }
                else if (role == "customer")
                {
                    stop = true;
                    int id = ATMManager.GetAccountNumber();
                    CustomerAccount acc = ATMManager.GetAccount(id);
                    if (acc.Status.Trim().ToLower() == "active")    //check if account is active OR blocked
                        CustomerView.MenuChoice();
                    else
                        Console.WriteLine("Your account is currently " + acc.Status + ". Contact Administration\n");
                }
                else if (role == "loginIssue")
                {
                    stop = true;
                    Console.WriteLine("No such user exist!\n");     //if username not matches
                }
                else if (role == "attemptsOverCustomer")
                {
                    stop = true;
                    Console.WriteLine("Pincode incorect!\n");       //customer entered incorrect pin 3 times
                    if (ATMManager.BlockAccount(cred.Login) > 0)
                        Console.WriteLine("Your session has terminated! Kindly Contact Administration to activate ypur account\n");
                    else
                        Console.WriteLine("couldn't block account\n");
                }
                else if (role == "attemptsOverAdmin")
                {
                    stop = true;
                    Console.WriteLine("Pincode incorect!\n");       //admmin entered incorrect pin 3 times 
                    Console.WriteLine("Your session has terminated!\n");
                }
                else if (role == "pinIssue")
                {
                    Console.WriteLine("Pincode incorect!\n");

                    Console.Write("Enter pin code (5-digit): ");    //again allow attempt
                    string pincode = Console.ReadLine();
                    pincode = CheckPinCode(pincode);

                    cred.Login = login.ToLower();
                    cred.PinCode = pincode;                         //take new pincode

                    //Console.WriteLine("login " + cred.Login);
                    //Console.WriteLine("login " + cred.PinCode);

                    role = ATMManager.CheckLoginBLL(cred);          //again check
                }
            }
        }
        internal static string CheckString(string s, string prompt)
        {
            /*
             * function validates if input string is empty OR not
             * arguments: username (to be validated), prompt(to re-enter input)
             * return: login/username
            */
            while (true)
            {
                if (s.All(c => char.IsLetterOrDigit(c)) && s != string.Empty)
                {
                    break;
                }
                Console.WriteLine("Invalid!\n");
                Console.Write(prompt+": ");
                s = Console.ReadLine();
            }
            return s;
        }
        internal static string CheckPinCode(string pincode)
        {
            /*
             * function validates input of pincode
             * arguments: pincode (to be validated)
             * return: pincode (valid)
            */
            while (!(uint.TryParse(pincode, out uint y) && pincode.Length == 5))
            {
                Console.WriteLine("Invalid PinCode!\n");
                Console.Write("Enter pin code (5-digit): ");
                pincode = Console.ReadLine();
            }
            return pincode;
        }
        internal static string CheckID(string accNumStr)
        {
            /*
            * function validates input of Account Number
            * arguments: account number (to be validated)
            * return: account number (valid)
           */
            while (true)
            {
                if (uint.TryParse(accNumStr, out uint y))
                    if (int.Parse(accNumStr) > 0)
                        break;
                Console.WriteLine("Invalid!\n");
                Console.Write("Account ID: ");
                accNumStr = Console.ReadLine();
            }
            return accNumStr;
        }
        internal static string CheckBalance(string bal, string s = "Balance")
        {
            /*
             * function peforms validation of balance
             * arguments: balance (to be validated)
             * return: valid balance input
            */
            bal = bal.Replace(",", "");         //if balance is of type 80,000
            while (true)
            {
                if (uint.TryParse(bal, out uint y))
                    if (int.Parse(bal) > 0)
                        break;
                Console.WriteLine("Invalid!\n");
                Console.Write(s + ": ");
                bal = Console.ReadLine();
                bal = bal.Replace(",", "");     //if balance is of type 80,000
            }
            return bal;
        }
        
    }
}
