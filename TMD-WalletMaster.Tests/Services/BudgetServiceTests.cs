using Moq;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Tests.Services
{
    public class BudgetServiceTests
    {
        private readonly Mock<IBudgetRepository> _mockBudgetRepository;
        private readonly IBudgetService _budgetService;

        public BudgetServiceTests()
        {
            _mockBudgetRepository = new Mock<IBudgetRepository>();
            _budgetService = new BudgetService(_mockBudgetRepository.Object);
        }

        [Fact]
        public async Task GetAllBudgetsAsync_ShouldReturnAllBudgets()
        {
            // Arrange
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Name = "Budget 1" },
                new Budget { Id = 2, Name = "Budget 2" }
            };
            
            _mockBudgetRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(budgets);

            // Act
            var result = await _budgetService.GetAllBudgetsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.Id == 1 && b.Name == "Budget 1");
            Assert.Contains(result, b => b.Id == 2 && b.Name == "Budget 2");
        }

        [Fact]
        public async Task GetBudgetByIdAsync_ShouldReturnBudget()
        {
            // Arrange
            var budget = new Budget { Id = 1, Name = "Budget 1" };

            _mockBudgetRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(budget);

            // Act
            var result = await _budgetService.GetBudgetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Budget 1", result.Name);
        }

        [Fact]
        public async Task CreateBudgetAsync_ShouldReturnCreatedBudget()
        {
            // Arrange
            var budget = new Budget { Id = 1, Name = "New Budget" };

            _mockBudgetRepository.Setup(repo => repo.AddAsync(budget)).Returns(Task.CompletedTask);

            // Act
            var result = await _budgetService.CreateBudgetAsync(budget);

            // Assert
            Assert.Equal(budget, result);
        }

        [Fact]
        public async Task UpdateBudgetAsync_ShouldReturnUpdatedBudget()
        {
            // Arrange
            var budget = new Budget { Id = 1, Name = "Updated Budget" };

            _mockBudgetRepository.Setup(repo => repo.UpdateAsync(budget)).Returns(Task.CompletedTask);

            // Act
            var result = await _budgetService.UpdateBudgetAsync(budget);

            // Assert
            Assert.Equal(budget, result);
        }

        [Fact]
        public async Task DeleteBudgetAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            _mockBudgetRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _budgetService.DeleteBudgetAsync(1);

            // Assert
            _mockBudgetRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
