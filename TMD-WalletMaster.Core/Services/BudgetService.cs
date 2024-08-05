using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<IEnumerable<Budget>> GetAllBudgetsAsync()
        {
            // Используем асинхронный метод репозитория для получения всех бюджетов
            return await _budgetRepository.GetAllAsync(); // Предполагается, что этот метод также асинхронный
        }

        public async Task<Budget> GetBudgetByIdAsync(int id)
        {
            // Используем асинхронный метод репозитория для получения бюджета по идентификатору
            return await _budgetRepository.GetByIdAsync(id);
        }

        public async Task<Budget> CreateBudgetAsync(Budget budget)
        {
            // Используем асинхронный метод репозитория для добавления нового бюджета
            await _budgetRepository.AddAsync(budget);
            return budget;
        }

        public async Task<Budget> UpdateBudgetAsync(Budget budget)
        {
            // Используем асинхронный метод репозитория для обновления существующего бюджета
            await _budgetRepository.UpdateAsync(budget);
            return budget;
        }

        public async Task DeleteBudgetAsync(int id)
        {
            // Используем асинхронный метод репозитория для удаления бюджета по идентификатору
            await _budgetRepository.DeleteAsync(id);
        }
    }
}