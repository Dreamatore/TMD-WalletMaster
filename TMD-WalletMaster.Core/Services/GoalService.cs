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
            // Используем асинхронный метод репозитория для получения всех целей
            return await _goalRepository.GetAllAsync(); // Предполагается, что этот метод также асинхронный
        }

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            // Используем асинхронный метод репозитория для получения цели по идентификатору
            return await _goalRepository.GetByIdAsync(id);
        }

        public async Task<Goal> CreateGoalAsync(Goal goal)
        {
            // Преобразование дат в UTC
            goal.StartDate = DateTime.SpecifyKind(goal.StartDate, DateTimeKind.Utc);
            goal.EndDate = DateTime.SpecifyKind(goal.EndDate, DateTimeKind.Utc);

            await _goalRepository.AddAsync(goal);
            return goal;
        }


        public async Task<Goal> UpdateGoalAsync(Goal goal)
        {
            // Используем асинхронный метод репозитория для обновления существующей цели
            await _goalRepository.UpdateAsync(goal);
            return goal;
        }

        public async Task DeleteGoalAsync(int id)
        {
            // Используем асинхронный метод репозитория для удаления цели по идентификатору
            await _goalRepository.DeleteAsync(id);
        }
        
    }
}