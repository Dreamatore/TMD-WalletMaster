using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task DeleteAllTransactionsByUserIdAsync(int userId)  // Добавлено
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
            if (transaction.Date.Kind == DateTimeKind.Unspecified)
            {
                transaction.Date = DateTime.SpecifyKind(transaction.Date, DateTimeKind.Utc);
            }

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

    }
}
