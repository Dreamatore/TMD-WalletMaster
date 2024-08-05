using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface IGoalService
    {
        Task<IEnumerable<Goal>> GetAllGoalsAsync(); 
        Task<Goal> GetGoalByIdAsync(int id);
        Task<Goal> CreateGoalAsync(Goal goal);
        Task<Goal> UpdateGoalAsync(Goal goal);
        Task DeleteGoalAsync(int id);
    }
}