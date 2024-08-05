using Moq;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services;
using TMD_WalletMaster.Core.Services.Interfaces;


namespace TMD_WalletMaster.Tests
{
    public class GoalServiceTests
    {
        private readonly Mock<IGoalRepository> _mockGoalRepository;
        private readonly IGoalService _goalService;

        public GoalServiceTests()
        {
            _mockGoalRepository = new Mock<IGoalRepository>();
            _goalService = new GoalService(_mockGoalRepository.Object);
        }

        [Fact]
        public async Task GetAllGoalsAsync_ShouldReturnAllGoals()
        {
            // Arrange
            var goals = new List<Goal>
            {
                new Goal { Id = 1, Name = "Goal 1" },
                new Goal { Id = 2, Name = "Goal 2" }
            };

            _mockGoalRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(goals);

            // Act
            var result = await _goalService.GetAllGoalsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, g => g.Id == 1 && g.Name == "Goal 1");
            Assert.Contains(result, g => g.Id == 2 && g.Name == "Goal 2");
        }

        [Fact]
        public async Task GetGoalByIdAsync_ShouldReturnGoal()
        {
            // Arrange
            var goal = new Goal { Id = 1, Name = "Goal 1" };

            _mockGoalRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(goal);

            // Act
            var result = await _goalService.GetGoalByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Goal 1", result.Name);
        }

        [Fact]
        public async Task CreateGoalAsync_ShouldReturnCreatedGoal()
        {
            // Arrange
            var goal = new Goal { Id = 1, Name = "New Goal" };

            _mockGoalRepository.Setup(repo => repo.AddAsync(goal)).Returns(Task.CompletedTask);

            // Act
            var result = await _goalService.CreateGoalAsync(goal);

            // Assert
            Assert.Equal(goal, result);
        }

        [Fact]
        public async Task UpdateGoalAsync_ShouldReturnUpdatedGoal()
        {
            // Arrange
            var goal = new Goal { Id = 1, Name = "Updated Goal" };

            _mockGoalRepository.Setup(repo => repo.UpdateAsync(goal)).Returns(Task.CompletedTask);

            // Act
            var result = await _goalService.UpdateGoalAsync(goal);

            // Assert
            Assert.Equal(goal, result);
        }

        [Fact]
        public async Task DeleteGoalAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            _mockGoalRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _goalService.DeleteGoalAsync(1);

            // Assert
            _mockGoalRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
