using System;
using System.Collections.Generic;
using System.Text;
using PL;

namespace CustomerPL
{
    public class ATMCustomerView:ATMView
    {
        public static void CustomerMenu()
        {
            Console.WriteLine("----Logged in as Customer----\n");
            Console.WriteLine("1----Withdraw Cash");
            Console.WriteLine("2----Transfer Cash");
            Console.WriteLine("3----Deposit Cash");
            Console.WriteLine("4----Display Balance");
            Console.WriteLine("5----Exit");

        }
        public static void MenuChoice()
        {
            string input = string.Empty;
            CustomerMenu();
            do
            {
                Console.Write("Please select one of the above options: ");
                input = Console.ReadLine();
                if (input == "1")
                {
                    //WithdrawCash();
                }
                else if (input == "2")
                {
                    //TransferCash();
                }
                else if (input == "3")
                {
                    //DepositCash();
                }
                else if (input == "4")
                {
                    //DisplayBalance();
                }
                else if (input != "5")
                {
                    Console.WriteLine("Invalid Input!");
                }
            } while (input != "5");
        }
       /* public static void WithdrawCash()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("a) Fast Cash");
                Console.WriteLine("b) Normal Cash");
                Console.Write("\nPlease select a mode of withdrawal: ");
                input = Console.ReadLine();
                if (input == "a" || input == "A")
                {
                    WithdrawFastCash();
                }
                else if (input == "b" || input == "B")
                {
                    WithdrawNormalCash();
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            } while (!(input == "a" || input == "b" || input == "A" || input == "b"));
        }
        internal void FastCashMenu()
        {
            Console.WriteLine("1----500");
            Console.WriteLine("2----1000");
            Console.WriteLine("3----2000");
            Console.WriteLine("4----5000");
            Console.WriteLine("5----10000");
            Console.WriteLine("6----15000");
            Console.WriteLine("7----20000");
        }
        public void WithdrawFastCash()
        {
            int choice = 0, cash = 0;
            do
            {
                FastCashMenu();
                Console.Write("Select one of the denominations of money: ");
                choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    cash = 500;
                }
                else if (choice == 2)
                {
                    cash = 1000;
                }
                else if (choice == 3)
                {
                    cash = 2000;
                }
                else if (choice == 4)
                {
                    cash = 5000;
                }
                else if (choice == 5)
                {
                    cash = 10000;
                }
                else if (choice == 6)
                {
                    cash = 15000;
                }
                else if (choice == 7)
                {
                    cash = 20000;
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                }
            } while (choice <= 0 || choice >= 8);

            Console.WriteLine($"Are you sure you want to withdraw Rs.{cash} (Y/N)? ");
            string input = Console.ReadLine();
            if (input == "Y" || input == "y")
            {
                ATMCustomerManager custmMng = new ATMCustomerManager();
                custmMng.WithdrawCash(cash);
            }
            else
            {

            }
        }
        public void WithdrawNormalCash()
        {

        }
        public void TransferCash()
        {

        }
        public void DepositCash()
        {

        }
        public void DisplayBalance()
        {

        }*/
    }
}
