namespace Test
{
    using System;
    using System.Threading;

    class Account
    {
        public decimal Balance { get; set; }

        public static void Transfer(Account from, Account to, decimal amount)
        {
            try
            {
                // 这里什么都不做
            }
            finally
            {
                from.Balance -= amount;
                to.Balance += amount;
            }
        }
    }

    class ThreadSafetyTest
    {
        private static bool isRunning = true;

        public static void Go()
        {
            Account accountA = new Account { Balance = 1000 };
            Account accountB = new Account { Balance = 1000 };

            Thread thread1 = new Thread(() => TransferTest(accountA, accountB));
            Thread thread2 = new Thread(() => TransferTest(accountB, accountA));

            thread1.Start();
            thread2.Start();

            // Run the test for a short duration
            Thread.Sleep(5000);
            isRunning = false;

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Final Balance of Account A: {accountA.Balance}");
            Console.WriteLine($"Final Balance of Account B: {accountB.Balance}");
            Console.WriteLine($"Total Balance: {accountA.Balance + accountB.Balance}");
        }

        private static void TransferTest(Account from, Account to)
        {
            Account.Transfer(from, to, 100);
        }
    }
}