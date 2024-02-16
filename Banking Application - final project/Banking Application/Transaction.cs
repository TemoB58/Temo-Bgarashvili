using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking_Application
{
    internal class Transaction
    {
        public string TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public double AmountGel { get; set; }
        public double AmountUSD { get; set; }
        public double AmountEUR { get; set; }
    }
}
