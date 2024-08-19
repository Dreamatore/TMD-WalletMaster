using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ApplicationDbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            if (transaction.Date.Kind == DateTimeKind.Unspecified)
            {
                transaction.Date = DateTime.SpecifyKind(transaction.Date, DateTimeKind.Utc);
            }

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<decimal> GetTotalAmountByUserIdAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .SumAsync(t => t.Amount);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllTransactionsByUserIdAsync(int userId)
        {
            var transactions = _context.Transactions.Where(t => t.UserId == userId);
            _context.Transactions.RemoveRange(transactions);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            return await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            _logger.LogInformation("Updating transaction with ID {TransactionId}.", transaction.Id);
    
            var existingTransaction = await _context.Transactions.FindAsync(transaction.Id);
            if (existingTransaction == null)
            {
                _logger.LogWarning("Transaction with ID {TransactionId} not found.", transaction.Id);
                return null;
            }

            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Description = transaction.Description;
            existingTransaction.Category = transaction.Category;
            _context.Transactions.Update(existingTransaction);
            _logger.LogInformation("Saving changes to database.");
            await _context.SaveChangesAsync();
    
            _logger.LogInformation("Transaction with ID {TransactionId} updated successfully.", transaction.Id);

            return existingTransaction;
        }
    }
}
