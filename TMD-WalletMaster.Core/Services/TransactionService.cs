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
            // Используем асинхронный метод репозитория для получения всех транзакций
            return await _transactionRepository.GetAllAsync(); // Предполагается, что этот метод также асинхронный
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            // Используем асинхронный метод репозитория для получения транзакции по идентификатору
            return await _transactionRepository.GetByIdAsync(id);
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            // Используем асинхронный метод репозитория для добавления новой транзакции
            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            // Используем асинхронный метод репозитория для обновления существующей транзакции
            await _transactionRepository.UpdateAsync(transaction);
            return transaction;
        }

        public async Task DeleteTransactionAsync(int id)
        {
            // Используем асинхронный метод репозитория для удаления транзакции по идентификатору
            await _transactionRepository.DeleteAsync(id);
        }
    }
}