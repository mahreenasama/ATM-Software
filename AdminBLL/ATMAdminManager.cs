using System;
using System.Collections.Generic;
using System.Text;
using BLL;
using AdminDAL;
using CustomerAccountBO;
using LoginCredentialsBO;

namespace AdminBLL
{
    public class ATMAdminManager:ATMManager
    {
        public static bool IsUserExist(LoginCredentials cred)
        {
            //encrypt credentials
            cred.Login = EncryptLogin(cred.Login);      // encrypt credentials then check
            cred.PinCode = int.Parse(EncryptLogin((cred.PinCode).ToString()));

            return ATMAdminDAL.IsUserExist(cred);
        }
        public static bool CreateLogin(LoginCredentials cred)
        {
            cred.Login = EncryptLogin(cred.Login);      // encrypt credentials then check
            cred.PinCode = int.Parse(EncryptLogin((cred.PinCode).ToString()));

            return ATMAdminDAL.CreateLogin(cred);
        }
        public static int CreateAccount(CustomerAccount account, string login)
        {
            login = EncryptLogin(login);
            return ATMAdminDAL.CreateAccount(account,login);
        }

      /*  public static string EncryptAdminLogin(string login)
        {
            int j = 0, nn = 0;
            string newLogin = "";
            foreach (char n in login)
            {
                if (n >= 65 && n <= 91)
                {
                    j = n - 65;
                    nn = (n - j + 25) - j;
                }
                else if (n >= 97 && n <= 122)
                {
                    Console.WriteLine("smalee");
                    j = n - 97;
                    nn = (n - j + 25) - j;
                }
                else if (n >= 48 && n <= 57)
                {
                    Console.WriteLine("numb");
                    // Console.WriteLine(n);

                    j = n - 48;
                    // Console.WriteLine(j);

                    nn = (n - j + 9) - j;
                    //Console.WriteLine(nn);
                }
                newLogin += (char)nn;
            }
            Console.WriteLine("new "+newLogin);
            return newLogin;
        }*/
    }
}
