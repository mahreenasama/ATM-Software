using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionsBO
{
    public class Transaction                    //saves data of transactions
    {
        public int ID { get; set; }
        public string Type { get; set; }        //withdraw,transfer,deposit
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public Transaction()
        {
            Date = DateTime.Today;
        }
    }
}
