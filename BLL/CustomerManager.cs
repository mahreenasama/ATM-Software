using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using TransactionsBO;

namespace BLL
{
    public class CustomerManager : ATMManager
    {
        //-----------------withdraw cash-----------
        public static int WithdrawCash(int cash, int prevBal)
        {
            /*
             * function acts as an intermediate (b/w View and DAL)for cash withdraw & keeping record of this transaction 
             * arguments: cash(to be withdrawn), previous Balance
             * return: number of transactions (recorded)
            */
            return CustomerDAL.WithdrawCash(cash,prevBal);
        }
        //-----------------transfer cash------------
        public static int TransferCash(int cash, int accNum)
        {
            /*
             * function acts as an intermediate (b/w View and DAL)for cash transfer & keeping record of this transaction 
             * arguments: cash(to transfer), account number(of person to which tranfer amount)
             * return: number of transactions (recorded)
            */
            return CustomerDAL.TransferCash(cash, accNum);
        }
        //-----------------deposit cash-----------
        public static int DepositCash(int cash)
        {
            /*
             * function acts as an intermediate (b/w View and DAL)for cash deposit & keeping record of this transaction 
             * arguments: cash(to be deposited)
             * return: number of transactions (recorded)
            */
            return CustomerDAL.DepositCash(cash);
        }
        //-----------------display balance------------
        public static void DisplayBalance(int cash)
        {
            //balance can be simply displayed by current users's account details 
        }

        //---helping functions
        public static int GetTodaysTotalWithdraw()
        {
            /*
             * function acts as an intermediate for calculating total cash withdrawn in current date
             * arguments: no
             * return: total amount (withdrawn by customer today)
            */
            return CustomerDAL.GetTodaysTotalWithdraw();
        }

    }
}
