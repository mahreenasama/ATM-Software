using System;
using System.Collections.Generic;
using System.Text;

namespace ReceiptBO
{
    public class Receipt
    {
        public int AccountNumber { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; }
        public string TransactionAmount { get; set; }
        public int BalanceCurr { get; set; }
    }
}
