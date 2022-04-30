using System;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var account = new BankAccount("Alan", 42000);
            Console.WriteLine($"Account {account.Number} was created for {account.Owner} with ${account.Balance}");

            account.MakeWithdrawal(420, DateTime.Now, "Weed");
            Console.WriteLine(account.Balance);

            // Test that the initial balances must be positive.
            BankAccount invalidAccount;
            try
            {
                invalidAccount = new BankAccount("invalid", -55);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Exception caught creating account with negative balance");
                Console.WriteLine(e.ToString());
                return;
            }

            account.MakeWithdrawal(420, DateTime.Now, "More Weed");
            Console.WriteLine(account.Balance);



        }
    }
}
