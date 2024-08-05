using Xunit;
using Moq;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace TMD_WalletMaster.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionService> _transactionServiceMock;

        public TransactionServiceTests()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
        }

        [Fact]
        public void GetAllTransactions_ShouldReturnTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Description = "Test Transaction 1", Amount = 50.00m },
                new Transaction { Id = 2, Description = "Test Transaction 2", Amount = 150.00m }
            };
            _transactionServiceMock.Setup(service => service.GetAllTransactions()).Returns(transactions);

            var service = _transactionServiceMock.Object;

            // Act
            var result = service.GetAllTransactions();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}