using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TMD_WalletMaster.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<UserService> _logger; 

        public UserService(IUserRepository userRepository, IBudgetRepository budgetRepository, ITransactionRepository transactionRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
            _transactionRepository = transactionRepository;
            _logger = logger; 
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userRepository.GetUserByUserNameAsync(userName);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int id)
        {
            return await _budgetRepository.GetBudgetsByUserIdAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionsByUserIdAsync(id);
        }

        public async Task<RegistrationResult> RegisterUserAsync(User user)
        {
            var result = new RegistrationResult();
            try
            {
                // Логирование попытки регистрации
                _logger.LogInformation("Attempting to register user: {UserName}", user.UserName);

                // Регистрация пользователя
                await _userRepository.AddAsync(user);
                result.Succeeded = true;

                // Логирование успешной регистрации
                _logger.LogInformation("User registered successfully: {UserName}", user.UserName);
            }
            catch (Exception ex)
            {
                // Логирование ошибки регистрации
                _logger.LogError(ex, "Failed to register user: {UserName}", user.UserName);

                result.Succeeded = false;
                result.Errors = new List<IdentityError> { new IdentityError { Description = ex.Message } };
            }
            return result;
        }
    }
}
