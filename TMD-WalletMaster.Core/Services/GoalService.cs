using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Goal>> GetAllGoalsAsync()
        {
            return await _goalRepository.GetAllAsync();
        }

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            return await _goalRepository.GetByIdAsync(id);
        }

        public async Task<Goal> CreateGoalAsync(Goal goal)
        {
            await _goalRepository.AddAsync(goal);
            return goal;
        }

        public async Task<Goal> UpdateGoalAsync(Goal goal)
        {
            await _goalRepository.UpdateAsync(goal);
            return goal;
        }

        public async Task DeleteGoalAsync(int id)
        {
            await _goalRepository.DeleteAsync(id);
        }
    }
}