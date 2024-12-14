using System;
using System.Collections.Generic;
using System.Text;
using LoginCredentialsBO;
using DAL;
using CustomerAccountBO;
using TransactionsBO;

namespace BLL
{
    public class AdminManager : ATMManager
    {
        //--------------------create account------------------
        public static int CreateAccount(CustomerAccount account, LoginCredentials cred)
        {
            /*
             * function acts as an intermediate for creating new Account b/w View and DAL
               (It encrypts login credentials and pass to DAL to save in DB)
             * arguments: Account & credential details (objects)
             * return: number of rows (added in table)
            */
            cred.Login = EncryptLogin(cred.Login);          //encrypt 
            cred.PinCode = EncryptLogin(cred.PinCode);

            return AdminDAL.CreateAccount(account, cred);
        }
        //---------------------delete account------------------------
        public static int DeleteAccount(int accNum)
        {
            /*
             * function acts as an intermediate for deleting Account b/w View and DAL
             * arguments: Account number (of account to be deleted)
             * return: number of rows (removed from table)
            */
            return AdminDAL.DeleteAccount(accNum);
        }
        //-------------------------update account---------------------
        public static int UpdateAccount(int accNum, CustomerAccount acc, LoginCredentials cred)
        {
            /*
             * function acts as an intermediate for updating Account and its credentials b/w View and DAL
               (It decrypts the data obtained from DAL)
             * arguments: Account number (of account to be updated), updated account & credential details
             * return: number of rows (updated in table)
            */
            cred.Login = EncryptLogin(cred.Login);
            cred.PinCode = EncryptLogin(cred.PinCode);
            return AdminDAL.UpdateAccount(accNum, acc, cred);
        }

        //-------------------------search accounts---------------------
        public static (List<CustomerAccount>, List<LoginCredentials>) SearchAccounts(List<string> searchList)
        {
            /*
            * function prepares query (to send to DAL) according to search criteria
            * arguments: list (of search criterias)
            * return: lists (of searched accounts & corresponding login credentials)
            */
            searchList[1] = EncryptLogin(searchList[1]);        //encrypt username
            //query to send 
            string query = $"select a.AccountID,ul.Login,ul.PinCode,a.HoldersName,a.Type,a.Balance,a.Status from Accounts a inner join UserLogin ul on a.AccountID=ul.accID";

            if (searchList[0] != string.Empty)
            {
                query += $" where a.AccountID={int.Parse(searchList[0])}";    //ANDing queries
            }
            if (searchList[1] != string.Empty)
            {
                if (searchList[0] == string.Empty)
                    query += $" where ul.Login=@user";
                else
                    query += $" and ul.Login=@user";
            }
            if (searchList[2] != string.Empty)
            {
                if (searchList[0] == string.Empty && searchList[1] == string.Empty)
                    query += $" where a.HoldersName=@name";
                else
                    query += $" and a.HoldersName=@name";
            }
            if (searchList[3] != string.Empty)
            {
                if(searchList[0] == string.Empty && searchList[1] == string.Empty && searchList[2] == string.Empty)
                    query += $" where a.Type='{searchList[3]}'";
                else
                    query += $" and a.Type='{searchList[3]}'";
            }
            if (searchList[4] != string.Empty)
            {
                if (searchList[0] == string.Empty && searchList[1] == string.Empty && searchList[2] == string.Empty && searchList[3] == string.Empty)
                    query += $" where a.Balance={searchList[4]}";
                else
                    query += $" and a.Balance={searchList[4]}";
            }
            if (searchList[5] != string.Empty)
            {
                if (searchList[0] == string.Empty && searchList[1] == string.Empty && searchList[2] == string.Empty && searchList[3] == string.Empty && searchList[4] == string.Empty)
                    query += $" where a.Status='{searchList[5]}'";
                else
                    query += $" and a.Status='{searchList[5]}'";
            }

            var data = AdminDAL.SearchAccounts(query, searchList);      //send to DAL

            List<CustomerAccount> accts = new List<CustomerAccount>();
            List<LoginCredentials> creds = new List<LoginCredentials>();
            accts = data.Item1;
            creds = data.Item2;
            foreach (LoginCredentials cred in creds)
            {
                cred.Login = EncryptLogin(cred.Login.Trim());          //decrypt credentials to display
                cred.PinCode = EncryptLogin(cred.PinCode);
            }
            return (accts, creds);
        }
        //-------------------------view reports---------------------
        public static (List<CustomerAccount>, List<LoginCredentials>) ViewReportsByAmount(int min, int max)
        {
            /*
           * function acts as an intermediate (b/w View and DAL) to prepare reports according to given Criteria (start & end date)
             (It decrypts the data obtained from DAL)
           * arguments: start,end dates
           * return: lists (of prepared reports)
           */
            var data = AdminDAL.ViewReportsByAmount(min, max);

            List<CustomerAccount> accts = new List<CustomerAccount>();
            List<LoginCredentials> creds = new List<LoginCredentials>();
            accts = data.Item1;
            creds = data.Item2;
            foreach(LoginCredentials cred in creds)
            {
                cred.Login = EncryptLogin(cred.Login.Trim());       //decrypt data to display
                cred.PinCode = EncryptLogin(cred.PinCode);
            }
            return (accts, creds);
        }
        public static (List<Transaction>, List<LoginCredentials>, List<string>) ViewReportsByDate(string start, string end, int accNum)
        {
            /*
           * function acts as an intermediate (b/w View and DAL) to prepare reports according to given Criteria (account #, start, end date)
             (It decrypts the data obtained from DAL)
           * arguments: minimum,maximum balance
           * return: lists (of prepared reports)
           */
            var data = AdminDAL.ViewReportsByDate(start, end, accNum);
            List<Transaction> trans = new List<Transaction>();
            List<LoginCredentials> creds = new List<LoginCredentials>();
            List<string> holders = new List<string>();
            trans = data.Item1;
            creds = data.Item2;
            holders = data.Item3;

            foreach (LoginCredentials cred in creds)
            {
                cred.Login = EncryptLogin(cred.Login.Trim());      //decrypt to display
                cred.PinCode = EncryptLogin(cred.PinCode);
            }
            return (trans, creds, holders);
        }

        //------helping functions
        public static bool IsUserExist(LoginCredentials cred)
        {
            /*
            * function acts as an intermediate b/w View and DAL, to check whether user with given credentials already exists in system or not
              (It encrypts credentials, then pass to DAL to check)
            * arguments: credentials (to be checked)
            * return: true(if user alreay exists), false otherwise
            */
            cred.Login = EncryptLogin(cred.Login);         //encrypt
            cred.PinCode = EncryptLogin(cred.PinCode);

            return AdminDAL.IsUserExist(cred);             //then pass to DAL
        }
        public static LoginCredentials GetCredentials(int accNum)
        {
            /*
           * function acts as an intermediate to get Login credentials b/w View and DAL
           * arguments: account number (of which credentials to get)
           * return: credentials (got, if found), null otherwise
           */
            LoginCredentials cred2 = AdminDAL.GetCredentials(accNum);
            cred2.Login = EncryptLogin(cred2.Login.Trim());
            cred2.PinCode = EncryptLogin(cred2.PinCode);
            return cred2;
        }

    }
}
