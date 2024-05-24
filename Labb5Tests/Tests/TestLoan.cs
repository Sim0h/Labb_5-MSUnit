using Syntax_Squad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxSquadTests
{
    public class TestLoan : Loan
    {
        public Action<double, int, int> AllLoanesAction { get; set; }

        public TestLoan(Transfer transfer, ExchangeRateManager exchangeRateManager)
            : base(transfer, exchangeRateManager)
        {
        }

        public override void AllLoanes(double loanS, int accountId, int toAcc)
        {
            AllLoanesAction?.Invoke(loanS, accountId, toAcc);
        }
    }
}

