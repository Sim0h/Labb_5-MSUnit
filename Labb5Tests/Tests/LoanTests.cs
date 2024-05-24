using Syntax_Squad;
namespace SyntaxSquadTests
{
    [TestClass]
    public class LoanTests
    {

        [TestMethod]
        public void TakeOutLoan_EnoughBalance_ShouldIncreaseLoanAmount()
        {
            // Arrange
            User user = new User { UserId = 1 };
            double initialBalance = 5000;
            BankAccount testAccount1 = new BankAccount("test", "test", 123, "user", 1, "SEK", initialBalance);

            var testTransfer = new TestTransfer
            {
                GetBankAccountFunc = accountNumber => testAccount1,
                LoggedInAccountListFunc = u => new List<int> { 123 }
            };

            var exchangeRateManager = new ExchangeRateManager();
            var testLoan = new TestLoan(testTransfer, exchangeRateManager);

            BankAccount.bankAccounts = new List<BankAccount>
        {
            testAccount1
        };

            double loanSize = 2000;
            double expectedLoanAmount = loanSize;

            bool allLoanesActionCalled = false;
            testLoan.AllLoanesAction = (loanS, accountId, toAcc) =>
            {
                allLoanesActionCalled = true;
                Assert.AreEqual(expectedLoanAmount, loanS, "Loan amount is not as expected.");
                Assert.AreEqual(user.UserId, accountId, "Account ID is not as expected.");
                Assert.AreEqual(testAccount1.AccountNumber, toAcc, "Target account number is not as expected.");
            };

            // Act
            testLoan.TakeOutLoan(user, loanSize);

            // Assert
            Assert.IsTrue(allLoanesActionCalled, "AllLoanes method was not called.");
            Assert.AreEqual(initialBalance, testAccount1.Balance - expectedLoanAmount, "User's total balance is not updated correctly.");
            Assert.AreEqual(initialBalance + expectedLoanAmount, testAccount1.Balance, "Target account balance is not updated correctly.");
        }

        [TestMethod]
        public void TakeOutLoan_NotEnoughBalance_ShouldNotIncreaseLoanAmount()
        {
            // Arrange
            User user = new User { UserId = 1 };
            double initialBalance = 100;
            BankAccount testAccount1 = new BankAccount("test", "test", 123, "user", 1, "SEK", initialBalance);

            var testTransfer = new TestTransfer
            {
                GetBankAccountFunc = accountNumber => testAccount1,
                LoggedInAccountListFunc = u => new List<int> { 123 }
            };

            var exchangeRateManager = new ExchangeRateManager();
            var testLoan = new TestLoan(testTransfer, exchangeRateManager);

            BankAccount.bankAccounts = new List<BankAccount>
        {
            testAccount1
        };

            double loanSize = 2000;
            double expectedLoanAmount = loanSize;

            bool allLoanesActionCalled = false;
            testLoan.AllLoanesAction = (loanS, accountId, toAcc) =>
            {
                allLoanesActionCalled = true;
                Assert.AreEqual(expectedLoanAmount, loanS, "Loan amount is not as expected.");
                Assert.AreEqual(user.UserId, accountId, "Account ID is not as expected.");
                Assert.AreEqual(testAccount1.AccountNumber, toAcc, "Target account number is not as expected.");
            };

            // Act
            testLoan.TakeOutLoan(user, loanSize);

            // Assert
            
            Assert.AreEqual(initialBalance, testAccount1.Balance - expectedLoanAmount, "User's total balance is not enough for a Loan.");
            Assert.AreEqual(initialBalance + expectedLoanAmount, testAccount1.Balance, "Target account balance is not updated correctly.");
        }


    }
}