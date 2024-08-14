using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly ITransactionRepository _transactionRepository;

        public UserService(IUserRepository userRepository, IBudgetRepository budgetRepository, ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(string id)
        {
            return await _budgetRepository.GetBudgetsByUserIdAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string id)
        {
            return await _transactionRepository.GetTransactionsByUserIdAsync(id);
        }
    }
}