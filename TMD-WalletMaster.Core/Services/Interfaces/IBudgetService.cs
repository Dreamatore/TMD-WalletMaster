using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(string userId);
        Task<IEnumerable<Budget>> GetAllBudgetsAsync();
        Task<Budget> GetBudgetByIdAsync(int id);
        Task<Budget> CreateBudgetAsync(Budget budget);
        Task<Budget> UpdateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int id);
    }
}