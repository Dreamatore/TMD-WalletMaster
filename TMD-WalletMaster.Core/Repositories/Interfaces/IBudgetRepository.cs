using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Repositories.Interfaces
{
    public interface IBudgetRepository
    {
        Task<IEnumerable<Budget>> GetAllAsync();
        Task<Budget> GetByIdAsync(int id);
        Task AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(int id);
        Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int userId);
    }
}