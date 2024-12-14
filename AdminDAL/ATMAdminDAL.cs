using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using LoginCredentialsBO;
using Microsoft.Data.SqlClient;
using CustomerAccountBO;

namespace AdminDAL
{
    public class ATMAdminDAL:ATMSoftwareDAL
    {
        /*public static new bool CheckLogin(LoginCredentials cred)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlParameter log = new SqlParameter("log", cred.Login);
            SqlParameter pin = new SqlParameter("pin", cred.PinCode);
            string query = "select * from UserLogin where Login=@log and Pincode=@pin and Type='admin'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(log);
            cmd.Parameters.Add(pin);
            conn.Open();                //connection open to run query
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;               //if admin not found
        }*/
        public static int CreateAccount(CustomerAccount account, string login)
        {
            /*//CreateLogin(cred.Login, cred.PinCode);
            int prevID = int.MinValue;
            SqlConnection conn = new SqlConnection(connStr);
            string query = "select max(AccountID) from Accounts";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                //Console.WriteLine((string)dr[0]);
                prevID = int.Parse((string)dr[0]);
                //conn.Close();
            }
            conn.Close();*/
            int prevID = GetAccountNumber();
            CreateCustomerAccount(account.HoldersName, account.Type, account.Balance, account.Status, login);
            return prevID + 1;
        }
        public static bool IsUserExist(LoginCredentials cred)
        {
            SqlConnection conn = new SqlConnection(connStr);

            SqlParameter log = new SqlParameter("log", cred.Login);
            string query = "select * from UserLogin where Login=@log";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(log);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }
        internal static int GetAccountNumber()
        {
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
                //conn.Close();
            }
            conn.Close();
            return accNum;
        }
        internal static void CreateCustomerAccount(string holder, string type, int bal, string status, string userID)
        {
            SqlConnection conn = new SqlConnection(connStr);

            SqlParameter holderName = new SqlParameter("holder", holder);   //parameters improve security
            SqlParameter UserID = new SqlParameter("userId", userID);
            SqlParameter Type = new SqlParameter("type", type);
            SqlParameter Balance = new SqlParameter("bal", bal);
            SqlParameter Status = new SqlParameter("status", status);

            string query = "insert into Accounts values (@userId, @holder, @type, @bal, @status)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(holderName);
            cmd.Parameters.Add(UserID);
            cmd.Parameters.Add(Type);
            cmd.Parameters.Add(Balance);
            cmd.Parameters.Add(Status);

            conn.Open();                //connection open to run query
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("ifff");
                conn.Close();
            }
            else
            {
                Console.WriteLine("else+ " + n);
                conn.Close();
            }
        }
        public static bool CreateLogin(LoginCredentials cred)
        {
            SqlConnection conn = new SqlConnection(connStr);

            Console.WriteLine("mang2 login: " + cred.Login);
            Console.WriteLine("mang2 pin: " + cred.PinCode);

            SqlParameter log = new SqlParameter("log", cred.Login);
            SqlParameter pin = new SqlParameter("pin", cred.PinCode);
            string query = "insert into UserLogin values (@log, @pin, 'customer')";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(log);
            cmd.Parameters.Add(pin);
            conn.Open();                //connection open to run query
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("ifff of create login");
                conn.Close();
                return true;
            }
            else
            {
                Console.WriteLine("else+ "+n);
                conn.Close();
                return false;
            }
        }
    }
}

