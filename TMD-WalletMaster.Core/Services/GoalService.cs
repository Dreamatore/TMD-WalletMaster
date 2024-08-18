using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;

        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId)
        {
            return await _goalRepository.GetGoalsByUserIdAsync(userId);
        }

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            return await _goalRepository.GetGoalByIdAsync(id);
        }

        public async Task<Goal> CreateGoalAsync(Goal goal)
        {
            return await _goalRepository.CreateGoalAsync(goal);
        }

        public async Task<Goal> UpdateGoalAsync(Goal goal)
        {
            return await _goalRepository.UpdateGoalAsync(goal);
        }

        public async Task DeleteGoalAsync(int id)
        {
            await _goalRepository.DeleteGoalAsync(id);
        }
    }
}