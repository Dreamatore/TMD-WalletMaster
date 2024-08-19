using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId); 
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id);
        Task DeleteAllTransactionsByUserIdAsync(int userId); 
        Task<decimal> GetTotalAmountByUserIdAsync(int userId);
    }
}