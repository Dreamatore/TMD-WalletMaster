using Xunit;
using Moq;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace TMD_WalletMaster.Tests.Services
{
    public class GoalServiceTests
    {
        private readonly Mock<IGoalService> _goalServiceMock;

        public GoalServiceTests()
        {
            _goalServiceMock = new Mock<IGoalService>();
        }

        [Fact]
        public void GetAllGoals_ShouldReturnGoals()
        {
            // Arrange
            var goals = new List<Goal>
            {
                new Goal { Id = 1, Name = "Test Goal 1", TargetAmount = 500.00m, CurrentAmount = 100.00m },
                new Goal { Id = 2, Name = "Test Goal 2", TargetAmount = 1000.00m, CurrentAmount = 200.00m }
            };
            _goalServiceMock.Setup(service => service.GetAllGoals()).Returns(goals);

            var service = _goalServiceMock.Object;

            // Act
            var result = service.GetAllGoals();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}