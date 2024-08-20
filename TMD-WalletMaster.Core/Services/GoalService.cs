using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TMD_WalletMaster.Core.Data;

namespace TMD_WalletMaster.Core.Services
{
    public class GoalService : IGoalService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GoalService> _logger;

        public GoalService(ApplicationDbContext context, ILogger<GoalService> logger)
        {
            _context = context;
            _logger = logger;
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
            _logger.LogInformation("Updating goal with ID {GoalId}.", goal.Id);

            var existingGoal = await _context.Goals.FindAsync(goal.Id);
            if (existingGoal == null)
            {
                _logger.LogWarning("Goal with ID {GoalId} not found.", goal.Id);
                return null;
            }

            existingGoal.Name = goal.Name;
            existingGoal.TargetAmount = goal.TargetAmount;
            existingGoal.CurrentAmount = goal.CurrentAmount;
            existingGoal.StartDate = goal.StartDate;
            existingGoal.EndDate = goal.EndDate;
            existingGoal.UserId = goal.UserId;

            _context.Goals.Update(existingGoal);
            _logger.LogInformation("Saving changes to database.");
            await _context.SaveChangesAsync();

            _logger.LogInformation("Goal with ID {GoalId} updated successfully.", goal.Id);

            return existingGoal;
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