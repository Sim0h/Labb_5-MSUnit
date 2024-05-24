using Syntax_Squad;
namespace SyntaxSquadTests
{
    [TestClass]
    public class TransferTests
    {
        [TestMethod] //klar
        public void TransferBetweenOwnAccounts_SameCurrency_ShouldTransfer()
        {
            // Arrange
            BankAccount testAccount1 = new BankAccount("test", "test", 123, "user", 1, "SEK", 100);
            BankAccount testAccount2 = new BankAccount("test", "test", 321, "user", 2, "SEK", 200);

            User user = new User
            {
                TransferLimit = 1000,
                Name = "user"
            };

            var transfer = new TestTransfer
            {
                GetBankAccountFunc = (accountNumber) => accountNumber == 123 ? testAccount1 : testAccount2,
                LoggedInAccountListFunc = (loggedInUser) => new List<int> { 123, 321 },
                TransferHistoryAction = (fromAcc, toAcc, currency, amount) => { }
            };

            // Act
            transfer.TransferBetweenOwnAccounts(user, 123, 321, 50);

            // Assert
            Assert.AreEqual(50, testAccount1.Balance, "Amount should be deducted from the source account.");
            Assert.AreEqual(250, testAccount2.Balance, "Amount should be added to the destination account.");
        }

        [TestMethod] //klar
        public void TransferBetweenOwnAccounts_SameCurrency_NegativeBalance_ShouldNotTransfer()
        {
            // Arrange
            BankAccount testAccount1 = new BankAccount("test1", "test", 123, "user", 1, "SEK", -100);
            BankAccount testAccount2 = new BankAccount("test2", "test", 321, "user", 1, "SEK", 200);

            User user = new User
            {
                TransferLimit = 1000,
                Name = "user",
                
            };

            var transfer = new TestTransfer
            {
                GetBankAccountFunc = (accountNumber) => accountNumber == 123 ? testAccount1 : testAccount2,
                LoggedInAccountListFunc = (loggedInUser) => new List<int> { 123, 321 },
                TransferHistoryAction = (fromAcc, toAcc, currency, amount) => { }
            };

            // Act
            transfer.TransferBetweenOwnAccounts(user, 123, 321, 50);

            // Assert
            Assert.AreEqual(-100, testAccount1.Balance, "Source account balance should remain unchanged.");
            Assert.AreEqual(200, testAccount2.Balance, "Destination account balance should remain unchanged.");
            Assert.AreEqual("Insufficient funds.", "Transfer should not be successful due to negative balance in source account.");
        }

        [TestMethod] // klar
        public void TransferBetweenOwnAccounts_DifferentCurrency_ShouldTransfer()
        {
            // Arrange
            BankAccount testAccount1 = new BankAccount("test1", "test1", 123, "user", 1, "SEK", 100);
            BankAccount testAccount2 = new BankAccount("test2", "test2", 321, "user", 1, "EUR", 200);

            User user = new User
            {
                TransferLimit = 1000,
                Name = "user"
            };

            var transfer = new TestTransfer
            {
                GetBankAccountFunc = (accountNumber) => accountNumber == 123 ? testAccount1 : testAccount2,
                LoggedInAccountListFunc = (loggedInUser) => new List<int> { 123, 321 },
                TransferHistoryAction = (fromAcc, toAcc, currency, amount) => { }
            };

            // Act
            transfer.TransferBetweenOwnAccounts(user, 123, 321, 50);

            // Assert
            Assert.AreEqual(50, testAccount1.Balance, "Amount should be deducted from the source account.");
            Assert.AreEqual(204.4, testAccount2.Balance, "Amount should be added to the destination account.");
        }

        [TestMethod] //klar
        public void TransferBetweenOtherAccounts_SameCurrency_ShouldTransfer()
        {
            // Arrange
            BankAccount testAccount1 = new BankAccount("test", "test", 123, "user1", 1, "SEK", 100);
            BankAccount testAccount2 = new BankAccount("test1", "test", 321, "user2", 2, "SEK", 200);

            User user1 = new User
            {
                TransferLimit = 1000,
                Name = "user1",
                Password = "test"

            };

           
            var transfer = new TestTransfer
            {
                GetBankAccountFunc = (accountNumber) => accountNumber == 123 ? testAccount1 : testAccount2,
                LoggedInAccountListFunc = (loggedInUser) => new List<int> { 123, 321 },
                TransferHistoryAction = (fromAcc, toAcc, currency, amount) => { }
            };

            // Act
            transfer.TransferBetweenOtherAccounts(user1, 123, 321, "test",50);

            // Assert
            Assert.AreEqual(50, testAccount1.Balance, "Amount should be deducted from the source account.");
            Assert.AreEqual(250, testAccount2.Balance, "Amount should be added to the destination account.");


        }

        [TestMethod] //klar
        public void TransferBetweenOtherAccounts_SameCurrency_ShouldNotTransfer()
        {
            // Arrange
            BankAccount testAccount1 = new BankAccount("test", "test", 123, "user1", 1, "SEK", -100);
            BankAccount testAccount2 = new BankAccount("test1", "test", 321, "user2", 2, "SEK", 200);

            User user1 = new User
            {
                TransferLimit = 1000,
                Name = "user1",
                Password = "test"

            };


            var transfer = new TestTransfer
            {
                GetBankAccountFunc = (accountNumber) => accountNumber == 123 ? testAccount1 : testAccount2,
                LoggedInAccountListFunc = (loggedInUser) => new List<int> { 123, 321 },
                TransferHistoryAction = (fromAcc, toAcc, currency, amount) => { }
            };

            // Act
            transfer.TransferBetweenOtherAccounts(user1, 123, 321, "test", 50);

            // Assert
            Assert.AreEqual(-100, testAccount1.Balance, "Amount should be deducted from the source account.");
            Assert.AreEqual(250, testAccount2.Balance, "Amount should be added to the destination account.");
        }

        [TestMethod] //klar
        public void TransferBetweenOtherAccounts_DifferentCurrency_ShouldTransfer()
        {
            // Arrange
            BankAccount testAccount1 = new BankAccount("test", "test", 123, "user1", 1, "SEK", 100);
            BankAccount testAccount2 = new BankAccount("test1", "test", 321, "user2", 2, "EUR", 200);

            User user1 = new User
            {
                TransferLimit = 1000,
                Name = "user1",
                Password = "test"

            };


            var transfer = new TestTransfer
            {
                GetBankAccountFunc = (accountNumber) => accountNumber == 123 ? testAccount1 : testAccount2,
                LoggedInAccountListFunc = (loggedInUser) => new List<int> { 123, 321 },
                TransferHistoryAction = (fromAcc, toAcc, currency, amount) => { }
            };

            // Act
            transfer.TransferBetweenOtherAccounts(user1, 123, 321, "test", 50);

            // Assert
            Assert.AreEqual(50, testAccount1.Balance, "Amount should be deducted from the source account.");
            Assert.AreEqual(204.4, testAccount2.Balance, "Amount should be added to the destination account.");
        }


    }
}