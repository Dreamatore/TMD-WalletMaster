using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int id); 
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int id); 
        Task<RegistrationResult> RegisterUserAsync(User user);
    }
}