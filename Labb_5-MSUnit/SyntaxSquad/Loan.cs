using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    public class Loan
    {
        private double loanAmount { get; set; }
        private int accountID { get; set; }
        private int accountNumber { get; set; }

        private static List<Loan> loans = new List<Loan>();
        private Transfer getBankInfo;
        private ExchangeRateManager exchangeRate;

        public Loan()
        {
            getBankInfo = new Transfer();
            exchangeRate = new ExchangeRateManager();
        }

        public Loan(Transfer transfer, ExchangeRateManager exchangeRateManager)
        {
            getBankInfo = transfer;
            exchangeRate = exchangeRateManager;
        }

        public virtual void TakeOutLoan(User user, double loanSize)
        {
            var convertedAmount = 0.0;
            double totalMoneyAmount = 0;
            foreach (BankAccount bankAccount in BankAccount.bankAccounts)
            {
                if (bankAccount.ID == user.UserId)
                {
                    var fromRate = Convert.ToDouble(exchangeRate.exchangeRates[bankAccount.Currency]);
                    var toRate = Convert.ToDouble(exchangeRate.exchangeRates["SEK"]);
                    convertedAmount = bankAccount.Balance * (1 / fromRate) * toRate;
                    totalMoneyAmount += convertedAmount;
                }
            }

            if (loanSize <= totalMoneyAmount * 5)
            {
                List<int> accNr = getBankInfo.loggedInAccountList(user);
                int toAcc = accNr.First();
                var toAccount = getBankInfo.GetBankAccount(toAcc);
                if (accNr.Contains(toAcc))
                {
                    var toRate = Convert.ToDouble(exchangeRate.exchangeRates[toAccount.Currency]);
                    var convertedLoan = loanSize * toRate;
                    toAccount.Balance += convertedLoan;
                    AllLoanes(loanSize, user.UserId, toAcc);
                    Console.WriteLine("Loan successfully taken.");
                }
            }
            else
            {
                Console.WriteLine("You do not have enough balance to take this loan.");
            }
        }

        public virtual void AllLoanes(double loanS, int accountId, int toAcc)
        {
            Loan newLoan = new Loan
            {
                loanAmount = loanS,
                accountID = accountId,
                accountNumber = toAcc
            };
            loans.Add(newLoan);
        }
    }

}
