using Moq;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly ITransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _transactionService = new TransactionService(_mockTransactionRepository.Object);
        }

        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnAllTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Description = "Transaction 1" },
                new Transaction { Id = 2, Description = "Transaction 2" }
            };

            _mockTransactionRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(transactions);

            // Act
            var result = await _transactionService.GetAllTransactionsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Id == 1 && t.Description == "Transaction 1");
            Assert.Contains(result, t => t.Id == 2 && t.Description == "Transaction 2");
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnTransaction()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, Description = "Transaction 1" };

            _mockTransactionRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(transaction);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Transaction 1", result.Description);
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldReturnCreatedTransaction()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, Description = "New Transaction" };

            _mockTransactionRepository.Setup(repo => repo.AddAsync(transaction)).Returns(Task.CompletedTask);

            // Act
            var result = await _transactionService.CreateTransactionAsync(transaction);

            // Assert
            Assert.Equal(transaction, result);
        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldReturnUpdatedTransaction()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, Description = "Updated Transaction" };

            _mockTransactionRepository.Setup(repo => repo.UpdateAsync(transaction)).Returns(Task.CompletedTask);

            // Act
            var result = await _transactionService.UpdateTransactionAsync(transaction);

            // Assert
            Assert.Equal(transaction, result);
        }

        [Fact]
        public async Task DeleteTransactionAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            _mockTransactionRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _transactionService.DeleteTransactionAsync(1);

            // Assert
            _mockTransactionRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
