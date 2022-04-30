using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Transaction
    {

        public decimal Amount { get; }
        public DateTime Date { get; }
        public string Notes { get; }

        public Transaction(decimal amount, DateTime date, string Note)
        {
            this.Amount = amount;
            this.Date = date;
            this.Notes = Note;
        }


    }
}
