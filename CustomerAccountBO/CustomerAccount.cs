using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAccountBO
{
    public class CustomerAccount
    {
        public int Id { get; set; }
        public string HoldersName { get; set; }
        public string Type { get; set; }
        public int Balance { get; set; }
        public string Status { get; set; }
    }
}
