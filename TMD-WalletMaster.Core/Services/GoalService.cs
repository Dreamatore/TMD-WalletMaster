using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Data;

namespace TMD_WalletMaster.Core.Services
{
    public class GoalService : IGoalService
    {
        private readonly ApplicationDbContext _context;

        public GoalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId)
        {
            return await _context.Goals
                .Where(g => g.UserId == userId)
                .ToListAsync();
        }

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            return await _context.Goals.FindAsync(id);
        }

        public async Task<Goal> CreateGoalAsync(Goal goal)
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return goal;
        }

        public async Task<Goal> UpdateGoalAsync(Goal goal)
        {
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();
            return goal;
        }

        public async Task DeleteGoalAsync(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAllGoalsByUserIdAsync(int userId)
        {
            var goals = await _context.Goals.Where(g => g.UserId == userId).ToListAsync();
            _context.Goals.RemoveRange(goals);
            await _context.SaveChangesAsync();
        }
    }
}