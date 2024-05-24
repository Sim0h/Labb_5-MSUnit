using Syntax_Squad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxSquadTests
{
    public class TestTransfer : Transfer
    {
        public Func<int, BankAccount> GetBankAccountFunc { get; set; }
        public Func<User, List<int>> LoggedInAccountListFunc { get; set; }
        public Action<int, int, string, double> TransferHistoryAction { get; set; }

        public override BankAccount GetBankAccount(int accountNumber)
        {
            return GetBankAccountFunc(accountNumber);
        }

        public override List<int> loggedInAccountList(User user)
        {
            return LoggedInAccountListFunc(user);
        }

        public override void transferHistory(int fromAccountNumber, int toAccountNumber, string currency, double amount)
        {
            TransferHistoryAction(fromAccountNumber, toAccountNumber, currency, amount);
        }
    }
}
