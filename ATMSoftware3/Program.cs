using System;
using PL;

namespace ATMSoftware3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----ATM----");
            ATMView.InputLoginInfo();           //input login info
            
            /*
             * For Admin (hardcoded some admin):-  
            Login: adnan123
            PinCode: 12345

            Note: AdminLogin is also saved in encrypted form
            */
        }
    }
}
