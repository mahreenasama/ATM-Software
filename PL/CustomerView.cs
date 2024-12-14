using System;
using System.Collections.Generic;
using System.Text;
using BLL;
using TransactionsBO;
using CustomerAccountBO;

namespace PL
{
    public class CustomerView : ATMView
    {
        internal static void CustomerMenu()
        {
            /*
             * function displays Menu choices of Customer
             * arguments: no
             * return: nothing
            */
            Console.WriteLine("\n----Logged in as Customer----\n");
            Console.WriteLine("1----Withdraw Cash");
            Console.WriteLine("2----Transfer Cash");
            Console.WriteLine("3----Deposit Cash");
            Console.WriteLine("4----Display Balance");
            Console.WriteLine("5----Exit");
        }
        public static void MenuChoice()
        {
            /*
             * function takes input of Customer's menu choice and calls respective functions
             * arguments: no
             * return: nothing
            */
            string input = string.Empty;
            do
            {
                CustomerMenu();
                Console.Write("\nPlease select one of the above options: ");
                input = Console.ReadLine();
                if (input == "1")
                {
                    WithdrawCash();                 //call for respective function
                }
                else if (input == "2")
                {
                    TransferCash();
                }
                else if (input == "3")
                {
                    DepositCash();
                }
                else if (input == "4")
                {
                    DisplayBalance();
                }
                else if (input != "5")
                {
                    Console.WriteLine("Invalid Input!\n");
                }
            } while (input != "5");
        }
        //--------------------------------withdraw cash----------------------
         public static void WithdrawCash()
         {
            /*
             * function takes input of Customer's Cash withdraw Mode and calls respective functions
             * arguments: no
             * return: nothing
            */
            string input = string.Empty;
            do
            {
                Console.WriteLine("\na) Fast Cash");
                Console.WriteLine("b) Normal Cash");
                Console.Write("\nPlease select a mode of withdrawal: ");
                input = Console.ReadLine();
                if (input.ToLower() == "a")
                {
                    WithdrawFastCash();
                }
                else if (input.ToLower() == "b")
                {
                    WithdrawNormalCash();
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            } while (!(input.ToLower() == "a" || input.ToLower() == "b"));
         }
         internal static void FastCashMenu()
         {
            /*
             * function displays Menu choices for Fast Cash Withdraw
             * arguments: no
             * return: nothing
            */
             Console.WriteLine("\n1----500");
             Console.WriteLine("2----1000");
             Console.WriteLine("3----2000");
             Console.WriteLine("4----5000");
             Console.WriteLine("5----10000");
             Console.WriteLine("6----15000");
             Console.WriteLine("7----20000");
         }
        internal static void WithdrawFastCash()
        {
            /*
             * function takes input of Customer's Fast Cash choice & asks for cash withdraw confirmation
             * arguments: no
             * return: nothing
            */
            string choice = string.Empty;
            int cash = 0;
            FastCashMenu();                            //fash cash menu (internal function)
            do
            {
                Console.Write("\nSelect one of the denominations of money: ");
                choice = Console.ReadLine();
                if (choice == "1")
                {
                    cash = 500;
                }
                else if (choice == "2")
                {
                    cash = 1000;
                }
                else if (choice == "3")
                {
                    cash = 2000;
                }
                else if (choice == "4")
                {
                    cash = 5000;
                }
                else if (choice == "5")
                {
                    cash = 10000;
                }
                else if (choice == "6")
                {
                    cash = 15000;
                }
                else if (choice == "7")
                {
                    cash = 20000;
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            } while (!(choice == "1" || choice =="2" || choice=="3" || choice=="4" || choice=="5" || choice=="6" || choice=="7"));

            Console.Write($"Are you sure you want to withdraw Rs.{cash} (Y/N)? ");
            string input = Console.ReadLine();
            while (!(input.ToLower() == "y" || input.ToLower() == "n"))
            {
                Console.WriteLine("Please enter correct input\n");
                Console.Write($"Are you sure you want to withdraw Rs.{cash} (Y/N)? ");
                input = Console.ReadLine();
            }
            if (input.ToLower() == "y")
            {
                WithdrawFinally(cash);                      //withdraw if user confirms
            }
        }
        internal static void WithdrawFinally(int cash)
        {
            /*
             * function checks for balance availability in account, then checks for amount withdraw limitations (in one day), and prints receipt if user wants
             * arguments: cash (to be withdrawn)
             * return: nothing
            */
            int accNum = ATMManager.GetAccountNumber();                 //of current customer
            int bal = ATMManager.GetAccount(accNum).Balance;
            if (bal >= cash)
            {
                int alreadyWithdrawnToday = CustomerManager.GetTodaysTotalWithdraw();
                if (alreadyWithdrawnToday+cash > 20000)                //withdrawn limitations per day
                {
                    //Console.WriteLine("You have already withdrwan Rs " + alreadyWithdrawnToday + " today");
                    Console.WriteLine("You cannot withdraw more than Rs 20,000 in a day\n");
                }
                else
                {
                    if (CustomerManager.WithdrawCash(cash, bal) > 0)
                    {
                        Console.WriteLine("Cash withdrawn successfully\n");
                        if (AskForReceipt() == "y")                       //if user wants to print receipt
                        {
                            CustomerAccount acc = ATMManager.GetAccount(ATMManager.GetAccountNumber());
                            PrintReceipt(acc.Id, "Cash Withdrawn", cash, acc.Balance);                //print receipt
                        }
                    }
                    else
                        Console.WriteLine("couldn't withdraw\n");
                }
            }
            else
            {
                Console.WriteLine("Sorry! your balance is less than this amount");
            }
        }
        internal static string AskForReceipt()
        {
            /*
             * function asks for receipt printing to customer (yes / no)
             * arguments: no
             * return: y/n (user's answer)
            */
            Console.Write("Do you wish to print a receipt (Y/N)? ");
            string input = Console.ReadLine();
            while (!(input.ToLower() == "y" || input.ToLower() == "n"))
            {
                Console.WriteLine("Please enter correct input\n");
                Console.Write("Do you wish to print a receipt (Y/N)? ");
                input = Console.ReadLine();
            }
            return input;
        }
        internal static void PrintReceipt(int id,string transacType, int amount, int remainBal)
        {
            /*
             * function prints receipt with given details 
             * arguments: transaction details (to be printed on receipt)
             * return: nothing
            */
            Console.WriteLine("\nAccount # " + id);
            Console.WriteLine("Date: " + DateTime.Now);
            Console.WriteLine(transacType + ": " + amount);
            Console.WriteLine("Balance: " + remainBal);
        }
        internal static void WithdrawNormalCash()
        {
            /*
             * function takes cash input for normal cash withdraw & withdraws(with the help of other internal functions)
             * arguments: no
             * return: nothing
            */
            Console.Write("Enter the withdrawal amount: ");
            string amount = Console.ReadLine();
            amount = CheckBalance(amount, "Enter the withdrawal amount");   //validate
            WithdrawFinally(int.Parse(amount));                  //+ print receipt 
        }

        //--------------------------------transfer cash----------------------
        public static void TransferCash()
        {
            /*
             * function takes all necessary inputs to transfer cash (to another account), & prints receipt if user wants
             * arguments: no
             * return: nothing
            */
            Console.Write("\nEnter amount in multiples of 500: ");
            string amount = Console.ReadLine();
            amount = CheckBalance(amount);                  //validate input
            int cash = int.Parse(amount);
            while(!(cash%500==0 && cash > 0))               //amount should be multiple of 500
            {
                Console.WriteLine("Invalid!\n");
                Console.Write("Enter amount in multiples of 500: ");
                amount = Console.ReadLine();
                amount = CheckBalance(amount);
                cash = int.Parse(amount);
            }
            //----
            Console.Write("\nEnter the account number to which you want to transfer: ");
            string accNumStr = Console.ReadLine();
            accNumStr = CheckID(accNumStr);                 //validate id
            CustomerAccount acc = ATMManager.GetAccount(int.Parse(accNumStr));
            if (acc == null)
            {
                Console.WriteLine("\nNo such Account exists\n");
            }
            else
            {
                Console.WriteLine("You wish to deposit Rs " + amount + " in account held by " + acc.HoldersName.Trim() + "; If this information is correct please re - enter the account number: ");
                string accNum2str = Console.ReadLine();
                while (accNum2str != accNumStr)
                {
                    Console.Write("Please re-enter same Account Number: ");
                    accNum2str = Console.ReadLine();
                }
                int accNum = ATMManager.GetAccountNumber();    //of current customer
                int bal = ATMManager.GetAccount(accNum).Balance;
                if (bal >= cash)
                {
                    if (CustomerManager.TransferCash(cash, int.Parse(accNumStr)) > 0)
                    {
                        Console.WriteLine("Transaction Confirmed\n");
                        if (AskForReceipt() == "y")             //print receipt
                        {
                            CustomerAccount acco = ATMManager.GetAccount(ATMManager.GetAccountNumber());
                            PrintReceipt(acco.Id, "Amount Transferred", cash, acco.Balance);
                        }
                    }
                    else
                        Console.WriteLine("couldn't transfer\n");
                }
                else
                {
                    Console.WriteLine("Sorry! your balance is less than this amount");
                }
            }
        }
        //--------------------------------deposit cash----------------------
        public static void DepositCash()
        {
            /*
             * function takes cash input to desposit, passes to BLL, and prints receipt if user wants
             * arguments: no
             * return: nothing
            */
            Console.Write("\nEnter the cash amount to deposit: ");
            string amount = Console.ReadLine();
            amount = CheckBalance(amount);                      //validate input
            int cash = int.Parse(amount);

            if (CustomerManager.DepositCash(cash) > 0)          //pass to BLL
            {
                Console.WriteLine("Cash Deposited successfully\n");
                if (AskForReceipt() == "y")                     //print receipt
                {
                    CustomerAccount acc = ATMManager.GetAccount(ATMManager.GetAccountNumber());
                    PrintReceipt(acc.Id, "Deposited", cash, acc.Balance);
                }
            }
            else
                Console.WriteLine("couldn't deposit cash");
         }
        //--------------------------------display balance----------------------
        public static void DisplayBalance()
         {
            /*
            * function displays account balance of currently logged-in customer
            * arguments: no
            * return: nothing
           */
            CustomerAccount acc = ATMManager.GetAccount(ATMManager.GetAccountNumber());
            Console.WriteLine("\nAccount # " + acc.Id);
            Console.WriteLine("Date: "+DateTime.Now);
            Console.WriteLine(
                    format: "Balance: {0:N0}",
                    arg0: acc.Balance
                    );
         }
    }
}
