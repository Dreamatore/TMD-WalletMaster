using System.Collections.Generic;
using System.Threading.Tasks;
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
            return await _budgetRepository.GetAllAsync();
        }

        public async Task<Budget> GetBudgetByIdAsync(int id)
        {
            return await _budgetRepository.GetByIdAsync(id);
        }

        public async Task<Budget> CreateBudgetAsync(Budget budget)
        {
            await _budgetRepository.AddAsync(budget);
            return budget;
        }

        public async Task<Budget> UpdateBudgetAsync(Budget budget)
        {
            await _budgetRepository.UpdateAsync(budget);
            return budget;
        }

        public async Task DeleteBudgetAsync(int id)
        {
            await _budgetRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(string userId)
        {
            return await _budgetRepository.GetBudgetsByUserIdAsync(userId);
        }
    }
}