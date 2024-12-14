using System;
using System.Collections.Generic;
using System.Text;
using LoginCredentialsBO;
using Microsoft.Data.SqlClient;
using CustomerAccountBO;

namespace DAL
{
    public class ATM_DAL     //base class
    {
        public const string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        internal static int id = 0;

        public static LoginCredentials CheckLoginDAL(LoginCredentials cred)
        {
            /*
             * function checks availability of credentials in DB 
             * arguments: login credentials
             * return: credentials object (matched from table) OR 'null' if no match found
            */
            LoginCredentials cred2 = null;

            SqlConnection conn = new SqlConnection(connStr);

            SqlParameter log = new SqlParameter("log", cred.Login);
            SqlParameter pin = new SqlParameter("pin", cred.PinCode);

            string query = "select * from UserLogin where Login=@log";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(log);
            cmd.Parameters.Add(pin);
            conn.Open();                //connection open to run query

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                cred2 = new LoginCredentials();
                cred2.Login = (string)dr[0];
                cred2.PinCode = (string)dr[1];
                cred2.Role = (string)dr[2];
                if (((string)dr[2]).Trim() == "customer")
                    id = (int)dr[3];
            }
            conn.Close();
            return cred2;             
        }

        //--------------------helping functions-----------------------------
        public static int GetAccountNumber()
        {
            /*
             * function returns Account number of currently logged-in customer 
             * arguments: no
             * return: account number
            */
            return id;
        }
        public static CustomerAccount GetAccount(int accNum)
        {
            /*
             * function returns Account details of a particular customer
             * arguments: account number
             * return: customer account details
            */
            CustomerAccount account = null;

            SqlConnection conn = new SqlConnection(connStr);

            string query = $"select * from Accounts where AccountID={accNum}";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                account = new CustomerAccount();
                dr.Read();
                account.Id = (int)dr[0];
                account.HoldersName = (string)dr[1];
                account.Type = (string)dr[2];
                account.Balance = (int)dr[3];
                account.Status = (string)dr[4];
            }
            conn.Close();
            return account;
        }
        public static int BlockAccount(string uname)
        {
            /*
             * function blocks the account status of customer (who enters 3 times invalid pincode)
             * arguments: usernmame (whose account to be blocked)
             * return: number of rows updated
            */
            int accNum = 0;
            SqlConnection conn = new SqlConnection(connStr);

            string query = $"select accID from UserLogin where Login=@uname";
            SqlParameter username = new SqlParameter("uname", uname);

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(username);

            conn.Open();                //connection open to run query
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                accNum = (int)dr[0];    //got ID against that username
            }
            conn.Close();

            //----------now update status
            query = $"update Accounts set Status='blocked' where AccountID={accNum}";
            cmd = new SqlCommand(query, conn);
            conn.Open();
            int n = cmd.ExecuteNonQuery();
            conn.Close();

            return n;                   //returning # of rows affected
        }
    }
}
