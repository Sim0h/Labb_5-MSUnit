using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.TimeZoneInfo;

namespace Syntax_Squad
{
    public class Transfer
    {
        //Simon Ståhl SUT23
        private List<BankAccount> transferAccounts = BankAccount.bankAccounts;

        public static List<Transfer> transactctionHistory = new List<Transfer>();
        ExchangeRateManager exchange = new ExchangeRateManager();
        private double amount { get; set; }
        private string Currency { get; set; }
        private int fromAccountNumber { get; set; }
        private int toAccountNumber { get; set; }
        private DateTime transactionTime { get; set; }


        /// <summary>
        /// Metod för överföring emellan egna konton. 
        /// PIN behövs inte för egen överföring då vi loggat in en gång redan.
        /// 
        /// </summary>

        public void TransferBetweenOwnAccounts(User user, int fromAccountNumber, int toAccountNumber, double amount)
        {
            List<int> loggedInUserAccountNumber = loggedInAccountList(user);

            try
            {
                var fromAccount = GetBankAccount(fromAccountNumber);
                var toAccount = GetBankAccount(toAccountNumber);

                if (user.TransferLimit >= amount || user.TransferLimit == 0)
                {
                    if (fromAccount.Balance >= amount && loggedInUserAccountNumber.Contains(fromAccountNumber))
                    {
                        if (fromAccount.Currency != toAccount.Currency)
                        {
                            if (exchange.exchangeRates.ContainsKey(fromAccount.Currency) && exchange.exchangeRates.ContainsKey(toAccount.Currency))
                            {
                                var fromRate = Convert.ToDouble(exchange.exchangeRates[fromAccount.Currency]);
                                var toRate = Convert.ToDouble(exchange.exchangeRates[toAccount.Currency]);
                                var convertedAmount = amount * (1 / fromRate) * toRate;
                                fromAccount.Balance -= amount;
                                toAccount.Balance += convertedAmount;
                                transferHistory(fromAccountNumber, toAccountNumber, fromAccount.Currency, amount);
                                Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                                Console.WriteLine($"\tNew balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}");
                            }
                        }
                        else
                        {
                            fromAccount.Balance -= amount;
                            toAccount.Balance += amount;
                            transferHistory(fromAccountNumber, toAccountNumber, fromAccount.Currency, amount);
                            Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                            Console.WriteLine($"\tNew balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"\tYou have exceeded your transaction limit.");
                    //Console.ReadKey();
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tInvalid input, please try again.");
            }
            //Console.ReadKey();
        }

        /// <summary>
        /// Metod för överföring från egna konton till andra användares konton. 
        /// PIN kontroll för att säkerställa att man vill föra över till annan användare. 
        /// </summary>

        public string TransferBetweenOtherAccounts(User user, int fromAccountNumber, int toAccountNumber, string password, double amount)
        {
            List<int> loggedInUserAccountNumber = loggedInAccountList(user);

            try
            {
                var fromAccount = GetBankAccount(fromAccountNumber);
                var toAccount = GetBankAccount(toAccountNumber);

                if (fromAccount == null || toAccount == null)
                {
                    Console.WriteLine("Invalid account number.");
                }

                if (password != user.Password)
                {
                    Console.WriteLine("Wrong password."); 
                }

                if (fromAccount.Balance <= 0)
                {
                    Console.WriteLine("Cannot transfer from account with negative balance.");
                }

                if (fromAccount.Balance < amount)
                {
                    Console.WriteLine("Insufficient funds.");
                }

                if (user.TransferLimit >= amount || user.TransferLimit == 0)
                {
                    if (fromAccount.Balance >= amount)
                    {
                        if (fromAccount.Currency != toAccount.Currency)
                        {
                            if (exchange.exchangeRates.ContainsKey(fromAccount.Currency) && exchange.exchangeRates.ContainsKey(toAccount.Currency))
                            {
                                var fromRate = Convert.ToDouble(exchange.exchangeRates[fromAccount.Currency]);
                                var toRate = Convert.ToDouble(exchange.exchangeRates[toAccount.Currency]);
                                var convertedAmount = amount * (1 / fromRate) * toRate;
                                fromAccount.Balance -= amount;
                                toAccount.Balance += convertedAmount;
                                transferHistory(fromAccountNumber, toAccountNumber, fromAccount.Currency, amount);
                                Console.WriteLine($"Transfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}. New balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}.");
                            }
                            else
                            {
                                return "Currency conversion rate not found.";
                            }
                        }
                        else
                        {
                            fromAccount.Balance -= amount;
                            toAccount.Balance += amount;
                            transferHistory(fromAccountNumber, toAccountNumber, fromAccount.Currency, amount);
                            Console.WriteLine($"Transfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}. New balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}.");
                        }
                    }
                }
                else
                {
                    return "You have exceeded your transaction limit.";
                }
            }
            catch (Exception)
            {
                return "Invalid input, please try again.";
            }

            return "An unknown error occurred.";
        }

        public virtual BankAccount GetBankAccount(int AccountNumber)
        {
            return BankAccount.bankAccounts.Find(a => a.AccountNumber == AccountNumber);
        }

        public virtual List<int> loggedInAccountList(User user)
        {
            List<int> loggedInUserAccountNumber = new List<int>();

            foreach (var account in BankAccount.bankAccounts)
            {
                if (account.Owner == user.Name)
                {
                    Console.WriteLine($"\tAccount Name: {account.AccountName}");
                    Console.WriteLine($"\tAccount number: {account.AccountNumber} Balance: {account.Balance}{account.Currency}");
                    loggedInUserAccountNumber.Add(account.AccountNumber);
                }
            }
            return loggedInUserAccountNumber;
        }

        public static void PrintTransferHistoryAdmin()
        {
           
            Console.WriteLine("\tTransaction History:");

            foreach (var transaction in transactctionHistory)
            {
                Console.WriteLine($"\tTransaction: From {transaction.fromAccountNumber} to {transaction.toAccountNumber} - Amount: {transaction.amount} {transaction.Currency} - Time: {transaction.transactionTime}");
            }

            Console.WriteLine($"\tPress enter to return to menu");
           

            Console.ReadKey();
        }

        public static void PrintTransactionHistoryUser(User user)
        {
            Console.Clear();
            foreach (var account in BankAccount.bankAccounts)
            {
                if (account.Owner == user.Name)
                {
                    foreach (var transaction in transactctionHistory)
                    {
                        if(transaction.fromAccountNumber == account.AccountNumber || transaction.toAccountNumber == account.AccountNumber)
                        {
                            Console.WriteLine($"\tTransaction: From {transaction.fromAccountNumber} to {transaction.toAccountNumber} - " +
                                $"Amount: {transaction.amount} {transaction.Currency} - Time: {transaction.transactionTime}");
                        }
                                            
                    }
                    break;
                }
            }

            Console.WriteLine($"\tPress enter to return to menu");

            

            Console.ReadKey();

        }

        public virtual void transferHistory(int fromAccount, int toAccount, string currency, double Amount)
        {
            Transfer transactionHistory = new Transfer
            {
                amount = Amount,
                Currency = currency,
                fromAccountNumber = fromAccount,
                toAccountNumber = toAccount,
                transactionTime = DateTime.Now
            };
            transactctionHistory.Add(transactionHistory);
        }

    }
}
