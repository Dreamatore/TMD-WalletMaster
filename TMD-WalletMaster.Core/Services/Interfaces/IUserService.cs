using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);
        Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(string id); // Объявите метод
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string id); // Объявите метод
    }
}