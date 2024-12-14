using System;
using System.Collections.Generic;
using System.Text;
using LoginCredentialsBO;
using DAL;
using CustomerAccountBO;

namespace BLL
{
    public class ATMManager         //base class
    {
        static int attempt = 0;     //to count invalid pincode attempts
        public static string CheckLoginBLL(LoginCredentials cred)
        {
            /*
             * function is an intermediate for checking Login credentials b/w View and DAL
               (encrypts Login credentials, passes to DAL (to check their availability), and decrypts received credentials)
             * arguments: login credentials
             * return: role against login
            */
            string login = cred.Login;
            string pin = cred.PinCode;
            cred.Login = EncryptLogin(login);                       //encrypt
            cred.PinCode = EncryptLogin(pin);

            LoginCredentials cred2 = ATM_DAL.CheckLoginDAL(cred);   //pass to DAL
            if (cred2 == null)
            {
                //Console.WriteLine("hl");
                return "loginIssue";                                //if username not exist
            }
            cred2.Login = EncryptLogin(cred2.Login.Trim());         //decrypt
            cred2.PinCode = EncryptLogin(cred2.PinCode);

            return CheckForAttempt(cred2, login, pin);              //check for number of attempts
        }

        //--------------helping functions------------
        public static CustomerAccount GetAccount(int accNum)
        {
            /*
             * function is an intermediate for getting Account details b/w View and DAL
             * arguments: account number (to get that particular account)
             * return: Account details (object)
            */
            return ATM_DAL.GetAccount(accNum);
        }
        public static int GetAccountNumber()
        {
           /*
            * function is an intermediate for getting Account number (of logged-in user) b/w View and DAL
            * arguments: no
            * return: account number
           */
            return ATM_DAL.GetAccountNumber();
        }
        public static int BlockAccount(string uname)
        {
            /*
            * function acts as an intermediate (b/w view & DAL) to block customer account
            * arguments: usernmame (whose account to be blocked)
            * return: number of rows updated
           */
            return ATM_DAL.BlockAccount(uname);
        }
        internal static string CheckForAttempt(LoginCredentials cred2, string login, string pin)
        {
            /*
             * function checks for valid number of attempts to user (if wrong pin entered 3 times)
             * arguments: login credentials
             * return: response about credentials accordingly
            */
            if (cred2.Login.ToLower() == login.ToLower() && cred2.PinCode == pin)
            {
                return cred2.Role.Trim();       //return role if valid user
            }
            else if (cred2.Login.ToLower() == login.ToLower() && cred2.PinCode != pin)
            {
                attempt++;
                if (attempt == 3 && cred2.Role.Trim()=="customer")
                    return "attemptsOverCustomer";
                else if (attempt == 3 && cred2.Role.Trim() == "admin")
                    return "attemptsOverAdmin";
                else
                    return "pinIssue";              //if attempts <3 (for customer)
            }
            else
            {
                return "loginIssue";
            }
        }
        internal static string EncryptLogin(string login)
        {
            /*
             * function encrypts passed string using an encryption technique(mentioned in assig. document)
             * arguments: string to be encrypted
             * return: encrypted string
            */
            int j = 0, nn = 0;
            string newLogin = "";
            foreach (var n in login)
            {
                if (n >= 65 && n <= 91)
                {
                    j = n - 65;
                    nn = (n - j + 25) - j;
                }
                else if (n >= 97 && n <= 122)
                {
                    j = n - 97;
                    nn = (n - j + 25) - j;
                }
                else if (n >= 48 && n <= 57)
                {
                    j = n - 48;
                    nn = (n - j + 9) - j;
                }
                newLogin += (char)nn;
            }
            //Console.WriteLine("new "+newLogin);
            return newLogin;
        }
    }
}
