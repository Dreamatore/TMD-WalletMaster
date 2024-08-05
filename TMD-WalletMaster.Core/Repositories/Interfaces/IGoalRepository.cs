using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Repositories.Interfaces
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetAllAsync();
        Task<Goal> GetByIdAsync(int id);
        Task AddAsync(Goal goal);
        Task UpdateAsync(Goal goal);
        Task DeleteAsync(int id);
    }
}