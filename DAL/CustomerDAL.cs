using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using TransactionsBO;

namespace DAL
{
    public class CustomerDAL : ATM_DAL
    {
        //---------------------WithdrawCash------------------------
        public static int WithdrawCash(int cash,int prevBal)
        {
            /*
             * function withdraws given cash (amount) for currenly logged-in customer & records this transaction
             * arguments: cash(to be withdrawn), previous Balance
             * return: number of transactions (recorded)
            */
            UpdateBalance(prevBal - cash, id);                     //after withdraw
            
            string d = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;

            string query = $"insert into Transactions (FromAcc,Type,Amount,Date) values ('{id}','Withdrawn','{cash}','{d}')";
            return InsertTransacrtion(query);                      //about amount withdrawn
        }
        internal static int UpdateBalance(int newBal, int accNum)
        {
            /*
             * function updates account balance of a particular account
             * arguments: account number(of which balance to be updated),new Balance
             * return: number of rows (updated in table)
            */
            SqlConnection conn = new SqlConnection(connStr);
            string query = $"update Accounts set Balance={newBal} where AccountID={accNum}";
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();                                //connection open to run query
            int n = cmd.ExecuteNonQuery();
            conn.Close();

            return n;
        }
        internal static int InsertTransacrtion(string query)
        {
            /*
             * function inserts a new transaction record in DB 
             * arguments: query(to run in DB)
             * return: number of rows (inserted in table)
            */
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();                                //connection open to run query
            int n = cmd.ExecuteNonQuery();
            conn.Close();

            return n;
        }
        //--------------------------transfer Cash------------------------------
        public static int TransferCash(int cash, int accNum)
        {
            /*
             * function transfers given cash (amount) from currently logged-in customer to given account number & records this transaction
             * arguments: cash(to transfer), account number(of person to which tranfer amount)
             * return: number of transactions (recorded)
            */
            int prevBal_From = GetAccount(id).Balance;
            UpdateBalance(prevBal_From - cash, id);             //of "cash from" cstomer
            int prevBal_To = GetAccount(accNum).Balance;
            UpdateBalance(prevBal_To + cash, accNum);           //of "cash To" customer

            string d = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;

            string query = $"insert into Transactions (FromAcc,Type,Amount,Date) values ({id},'Transferred',{cash},'{d}')";
            return InsertTransacrtion(query);                   //record this transaction
        }

        //-------------------------deposit Cash----------------------------------
        public static int DepositCash(int cash)
        {
            /*
             * function deposits given cash (amount) into currenly logged-in customer's account & records this transaction
             * arguments: cash(to be deposited)
             * return: number of transactions (recorded)
            */
            int prevBal = GetAccount(id).Balance;
            UpdateBalance(prevBal + cash, id);          //update balance of current user

            string d = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;

            string query = $"insert into Transactions values ({id},'Deposited',{cash},'{d}')";
            return InsertTransacrtion(query);           //record transaction
        }

        //--------------helping functions
        public static int GetTodaysTotalWithdraw()
        {
            /*
             * function calculates total amount withdrawn by currernt customer at current date
             * arguments: no
             * return: total amount (withdrawn by customer today)
            */
            int amount = 0;
            SqlConnection conn = new SqlConnection(connStr);

            string d = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;

            string query = $"select Amount from Transactions where FromAcc={id} and Type='Withdrawn' and Date='{d}'";
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();                            //connection open to run query
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //Console.WriteLine("here");
                amount += (int)dr[0];               //adding todays withdrwan amounts
            }
            conn.Close();
            //Console.WriteLine("am "+amount);
            return amount;
        }
    }
}
