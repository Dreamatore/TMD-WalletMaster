using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Repositories.Interfaces;

namespace TMD_WalletMaster.Core.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Budget>> GetAllAsync()
        {
            return await _context.Budgets.ToListAsync();
        }

        public async Task<Budget> GetByIdAsync(int id)
        {
            return await _context.Budgets.FindAsync(id);
        }

        public async Task AddAsync(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(string userId)
        {
            return await _context.Budgets
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}