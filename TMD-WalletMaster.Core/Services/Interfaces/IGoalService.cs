using TMD_WalletMaster.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface IGoalService
    {
        Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId); 
        Task<Goal> GetGoalByIdAsync(int id);
        Task<Goal> CreateGoalAsync(Goal goal);
        Task<Goal> UpdateGoalAsync(Goal goal);
        Task DeleteGoalAsync(int id);
        Task DeleteAllGoalsByUserIdAsync(int userId);
    }
}