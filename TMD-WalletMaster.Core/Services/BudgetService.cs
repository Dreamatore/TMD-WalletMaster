
using Microsoft.Extensions.Logging;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBudgetRepository _budgetRepository;
        private readonly ILogger<BudgetService> _logger;

        public BudgetService(IBudgetRepository budgetRepository,ILogger<BudgetService>logger,ApplicationDbContext context)
        {
            _budgetRepository = budgetRepository;
            _logger = logger;
            _context = context;
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
            _logger.LogInformation("Updating budget with ID {BudgetId}.", budget.Id);

            var existingBudget = await _context.Budgets.FindAsync(budget.Id);
            if (existingBudget == null)
            {
                _logger.LogWarning("Budget with ID {BudgetId} not found.", budget.Id);
                return null;
            }

            existingBudget.Name = budget.Name;
            existingBudget.Amount = budget.Amount;
            existingBudget.StartDate = budget.StartDate;
            existingBudget.EndDate = budget.EndDate;
            existingBudget.CategoryId = budget.CategoryId;

            _context.Budgets.Update(existingBudget);
            _logger.LogInformation("Saving changes to database.");
            await _context.SaveChangesAsync();

            _logger.LogInformation("Budget with ID {BudgetId} updated successfully.", budget.Id);

            return existingBudget;
        }


        public async Task DeleteBudgetAsync(int id)
        {
            await _budgetRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int userId)
        {
            return await _budgetRepository.GetBudgetsByUserIdAsync(userId);
        }
        public async Task DeleteAllBudgetsByUserIdAsync(int userId)
        {
            var budgets = await _budgetRepository.GetBudgetsByUserIdAsync(userId);
            foreach (var budget in budgets)
            {
                await _budgetRepository.DeleteAsync(budget.Id);
            }
        }

    }
}