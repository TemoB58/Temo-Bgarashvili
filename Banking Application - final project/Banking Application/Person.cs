using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Banking_Application
{
    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
        public string PinCode { get; set; }
        public string CVC { get; set; }
        public string ExpirationDate { get; set; }
        public List<Transaction> TransactionHistory { get; set; }
    }
}
