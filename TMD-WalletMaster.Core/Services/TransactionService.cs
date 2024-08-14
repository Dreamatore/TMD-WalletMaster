using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateAsync(transaction);
            return transaction;
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId)
        {
            return await _transactionRepository.GetTransactionsByUserIdAsync(userId);
        }
    }
}