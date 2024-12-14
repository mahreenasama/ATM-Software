using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using LoginCredentialsBO;
using CustomerAccountBO;
using TransactionsBO;

namespace DAL
{
    public class AdminDAL : ATM_DAL
    {
        //-----------------------------create account--------------------------
        public static int CreateAccount(CustomerAccount account, LoginCredentials cred)
        {
            /*
             * function creates new account and corrrsponding login credentials (with the help of other functions)
             * arguments: account & login details
             * return: account number (of newly created account)
            */
            CreateCustomerAccount(account);          //create account 
            int accNum = GetLatestAccountNumber();
            CreateLogin(cred,accNum);                //create login credentials

            return accNum;
        }
        internal static int CreateLogin(LoginCredentials cred, int accNum)
        {
            /*
             * function creates login credentials for a particular customer
             * arguments: credetial details, account number (for which credentials are to be created)
             * return: number of rows (added in table)
            */
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"insert into UserLogin values (@log, @pin, 'customer', {accNum})";

            SqlParameter log = new SqlParameter("log", cred.Login);
            SqlParameter pin = new SqlParameter("pin", cred.PinCode);

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(log);
            cmd.Parameters.Add(pin);

            conn.Open();                            //connection open to run query
            int n = cmd.ExecuteNonQuery();
            conn.Close();

            return n;                               //return # of rows affected
        }
        internal static int CreateCustomerAccount(CustomerAccount acc)
        {
            /*
             * function creates new account (adds to DB table)
             * arguments: Account details (object)
             * return: number of rows (added in table)
            */
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"insert into Accounts values (@holder, '{acc.Type}', {acc.Balance}, '{acc.Status}')";

            SqlParameter holderName = new SqlParameter("holder", acc.HoldersName);   //parameters improve security
            
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(holderName);

            conn.Open();                            //connection open to run query
            int n = cmd.ExecuteNonQuery();
            conn.Close();

            return n;                               //return # of rows affected
        }
        internal static int GetLatestAccountNumber()
        {
            /*
             * function returns largest account number (latest created account) from DB
             * arguments: no
             * return: account number (of last account in DB table)
            */
            int accNum = 0;
            SqlConnection conn = new SqlConnection(connStr);
            string query = "select max(AccountID) from Accounts";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                accNum = (int)dr[0];
            }
            conn.Close();
            return accNum;
        }
        //-------------------------------delete account------------------------
        public static int DeleteAccount(int accNum)
        {
            /*
             * function deletes account, all its transactions and its credentials from DB 
             * arguments: Account number (of account to be deleted)
             * return: number of rows (removed from table)
            */
            DeleteCredentials(accNum);                      //delete credentials
            DeleteTransactions(accNum);                     //delete transactions
            return DeleteCustomerAccount(accNum);           //delete account
        }
        internal static int DeleteTransactions(int accNum)
        {
            /*
            * function deletes all transactions of a particular account from DB
            * arguments: Account number (of which transactions to be deleted)
            * return: number of rows (removed from table)
           */
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"delete from Transactions where FromAcc={accNum}";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();                            //conn. open to run query
            int n = cmd.ExecuteNonQuery();              
            conn.Close();

            return n;                               //return # of rows removed 
        }
        internal static int DeleteCredentials(int accNum)
        {
           /*
           * function deletes login credentials of a particular account from DB
           * arguments: Account number (of which credentials to be deleted)
           * return: number of rows (removed from table)
           */
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"delete from UserLogin where Type='customer' and accID={accNum}";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();
            int n = cmd.ExecuteNonQuery();                  //credentials delete
            conn.Close();

            return n;
        }
        internal static int DeleteCustomerAccount(int accNum)
        {
           /*
           * function deletes a particular account from DB
           * arguments: Account number (of account to be deleted)
           * return: number of rows (removed from table)
           */
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"delete from Accounts where AccountID={accNum}";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();
            int n = cmd.ExecuteNonQuery();          //account delete
            conn.Close();

            return n;                               //retuning number of rows affected
        }

        //-------------------------update account--------------------------
        public static int UpdateAccount(int accNum, CustomerAccount acc, LoginCredentials cred)
        {
            /*
            * function updates account & credential details of a particular account
            * arguments: Account number (to be updated)
            * return: number of rows (updated in table)
            */
            UpdateCredentials(cred, accNum);                    //update credentials
            return UpdateCustomerAccount(acc, accNum);          //update account
        }
        internal static int UpdateCredentials(LoginCredentials cred, int accNum)
        {
            /*
            * function updates credential details of a particular account
            * arguments: new credentials, Account number (of which credentials to be updated)
            * return: number of rows (updated in table)
            */
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"Update UserLogin set Login=@login, PinCode=@pin where AccID={accNum}";

            SqlParameter name = new SqlParameter("login", cred.Login);
            SqlParameter status = new SqlParameter("pin", cred.PinCode);

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(name);
            cmd.Parameters.Add(status);

            conn.Open();
            int n = cmd.ExecuteNonQuery();
            conn.Close();

            return n;
        }
        internal static int UpdateCustomerAccount(CustomerAccount acc, int accNum)
        {
            /*
            * function updates account details of a particular account
            * arguments: updated account details, Account number (to be updated)
            * return: number of rows (updated in table)
            */
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"update Accounts set HoldersName=@name, Status='{acc.Status}' where AccountID={accNum}";
            SqlParameter name = new SqlParameter("name", acc.HoldersName);

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(name);

            conn.Open();
            int n = cmd.ExecuteNonQuery();
            conn.Close();

            return n;
        }
        //---------------------------------search-----------------------------
        public static (List<CustomerAccount>, List<LoginCredentials>) SearchAccounts(string query, List<string> searchCriteria)
        {
            /*
            * function searches for accounts in DB accoording to searchCriteria (received in list)
            * arguments: list (of search criterias), particular query(accrding to cearach criteria)
            * return: lists (of searched accounts & corresponding login credentials)
            */
            List<CustomerAccount> AccList = new List<CustomerAccount>();
            List<LoginCredentials> LoginList = new List<LoginCredentials>();

            SqlConnection conn = new SqlConnection(connStr);

            SqlParameter userID = new SqlParameter("user", searchCriteria[1]);
            SqlParameter name = new SqlParameter("name", searchCriteria[2]);

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(userID);
            cmd.Parameters.Add(name);

            conn.Open();

            CustomerAccount acc = null;
            LoginCredentials cred = null;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                acc = new CustomerAccount();            //creating new objects
                cred = new LoginCredentials();

                acc.Id = (int)dr[0];                    //adding info to objects     
                cred.Login = (string)dr[1];
                cred.PinCode = (string)dr[2];
                acc.HoldersName = (string)dr[3];
                acc.Type = (string)dr[4];
                acc.Balance = (int)dr[5];
                acc.Status = (string)dr[6];

                AccList.Add(acc);                       //adding objects to lists
                LoginList.Add(cred);
            }
            conn.Close();

            return (AccList, LoginList);                //returning lists
        }

        //----------------------------view reports------------------------
        public static (List<Transaction>,List<LoginCredentials>, List<string>) ViewReportsByDate(string start, string end, int accNum)
        {
            /*
           * function prepares report according to given Criteria (account #, start & end date)
           * arguments: start,end dates
           * return: lists (of prepared reports)
           */
            List<Transaction> TransList = new List<Transaction>();
            List<LoginCredentials> LoginList = new List<LoginCredentials>();
            List<string> accHolders = new List<string>();

            SqlConnection conn = new SqlConnection(connStr);

            string[] starts = start.Split("/");
            string[] ends = end.Split("/");
            start = starts[2] + "-" + starts[1] + "-" + starts[0];
            end = ends[2] + "-" + ends[1] + "-" + ends[0];

            string query = $"select t.Type,c.Login,c.PinCode,c.HoldersName,t.Amount,t.Date from (select a.AccountID,a.HoldersName,ul.Login,ul.PinCode from Accounts a inner join UserLogin ul on a.AccountID=ul.accID where a.AccountID={accNum}) c inner join Transactions t on c.AccountID=t.FromAcc where t.Date BETWEEN '{start}' AND '{end}'";
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            Transaction trans = null;
            LoginCredentials cred = null;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                trans = new Transaction();                  //creating  objects
                cred = new LoginCredentials();

                trans.Type = (string)dr[0];                 //adding data to objects
                cred.Login = (string)dr[1];
                cred.PinCode = (string)dr[2];
                trans.Amount = (int)dr[4];
                trans.Date = (DateTime)dr[5];

                TransList.Add(trans);                       //adding objects to lists
                LoginList.Add(cred);
                accHolders.Add((string)dr[3]);
            }
            conn.Close();

            return (TransList, LoginList, accHolders);      //returning lists
        }
        public static (List<CustomerAccount>, List<LoginCredentials>) ViewReportsByAmount(int min, int max)
        {
            /*
           * function prepares report according to given Criteria (min & max balance)
           * arguments: minimum,maximum balance
           * return: lists (of prepared reports)
           */
            List<CustomerAccount> AccList = new List<CustomerAccount>();
            List<LoginCredentials> LoginList = new List<LoginCredentials>();

            SqlConnection conn = new SqlConnection(connStr);

            string query = $"select a.AccountID,ul.Login,ul.PinCode,a.HoldersName,a.Type,a.Balance,a.Status from Accounts a inner join UserLogin ul on a.AccountID=ul.accID where a.Balance BETWEEN {min} AND {max}";
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            CustomerAccount acc = null;              
            LoginCredentials cred = null;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                acc = new CustomerAccount();                //creating objects 
                cred = new LoginCredentials();

                acc.Id = (int)dr[0];                        //adding data to objects
                cred.Login = (string)dr[1];
                cred.PinCode = (string)dr[2];
                acc.HoldersName = (string)dr[3];
                acc.Type = (string)dr[4];
                acc.Balance = (int)dr[5];
                acc.Status = (string)dr[6];

                AccList.Add(acc);                           //adding objects to lists
                LoginList.Add(cred);
            }
            conn.Close();

            return (AccList, LoginList);                    //returning lists
        }

        //---helping functionss
        public static bool IsUserExist(LoginCredentials cred)
        {
            /*
           * function checks whether a user with given username already exists in system
           * arguments: credentials (to be checked)
           * return: true(if user alreay exists), false otherwise
           */
            SqlConnection conn = new SqlConnection(connStr);

            SqlParameter log = new SqlParameter("log", cred.Login);
            string query = "select * from UserLogin where Login=@log";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(log);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            bool userExist = dr.HasRows;                //if user with same username already exists
            conn.Close();

            return userExist;
        }
        public static LoginCredentials GetCredentials(int accNum)
        {
            /*
           * function returns login credentials of particluar account number
           * arguments: account number (of which credentials to get)
           * return: credentials (got, if found), null otherwise
           */
            LoginCredentials cred = null;

            SqlConnection conn = new SqlConnection(connStr);

            string query = $"select * from UserLogin where accID={accNum}";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                cred = new LoginCredentials();
                cred.Login = (string)dr[0];
                cred.PinCode = (string)dr[1];
                cred.Role = (string)dr[2];
            }
            conn.Close();
            return cred;
        }
    }
}
