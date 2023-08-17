using Moq;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Models;
using Xunit;

namespace YourNamespace.Tests.Services
{
    public class HomeRepositoryTests
    {
        [Theory]
        [InlineData(100, 100, TransactionType.T, false)]
        [InlineData(200, 100, TransactionType.W, false)]
        public void ValidateAndStoreTransaction_Should_Return_Correct_Result(float balance, float amount, TransactionType transactionType, bool expectedResult)
        {
            // Arrange
            var mockDataAccess = new Mock<IDataAccessRepository>(); // Mock the IDataAccess dependency
            var homeRepository = new HomeRepository(mockDataAccess.Object); // Pass the mock to the HomeRepository

            // Set up the behavior of the mocked IDataAccess
            var accountNumber = 123; // Replace with the actual account number
            var transactions = new List<Transaction>
            {
                new Transaction { TransactionType = TransactionType.S },
                new Transaction { TransactionType = TransactionType.T },
                // Add more sample transactions as needed
            };
            mockDataAccess.Setup(dataAccess => dataAccess.GetTransactionsByAccountNumber(accountNumber)).Returns(transactions);

            // Create a sample transaction for testing
            var transaction = new Transaction
            {
                AccountNumber = accountNumber,
                TransactionType = TransactionType.T,
                // Set other properties as needed
            };

            // Act
            var result = homeRepository.ValidateAndStoreTransaction(transaction);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
