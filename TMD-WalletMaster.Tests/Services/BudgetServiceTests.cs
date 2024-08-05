using Xunit;
using Moq;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace TMD_WalletMaster.Tests.Services
{
    public class BudgetServiceTests
    {
        private readonly Mock<IBudgetService> _budgetServiceMock;

        public BudgetServiceTests()
        {
            _budgetServiceMock = new Mock<IBudgetService>();
        }

        [Fact]
        public void GetAllBudgets_ShouldReturnBudgets()
        {
            // Arrange
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Name = "Test Budget 1", Amount = 100.00m },
                new Budget { Id = 2, Name = "Test Budget 2", Amount = 200.00m }
            };
            _budgetServiceMock.Setup(service => service.GetAllBudgets()).Returns(budgets);

            var service = _budgetServiceMock.Object;

            // Act
            var result = service.GetAllBudgets();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}