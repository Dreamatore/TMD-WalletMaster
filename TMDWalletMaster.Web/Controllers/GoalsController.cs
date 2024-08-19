using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMDWalletMaster.Web.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;
        private readonly ILogger<GoalsController> _logger;

        public GoalsController(IGoalService goalService, ILogger<GoalsController> logger)
        {
            _goalService = goalService;
            _logger = logger;
        }

        // GET: /Goals
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is not authenticated. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("UserId is not valid. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation($"Fetching goals for user with ID: {userId}");

            var goals = await _goalService.GetGoalsByUserIdAsync(userId);
            _logger.LogInformation($"Fetched {goals.Count()} goals for user with ID: {userId}");

            return View(goals);
        }

        // GET: /Goals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Goals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Goal goal)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                {
                    _logger.LogWarning("UserId is not valid. Redirecting to login page.");
                    return RedirectToAction("Login", "Account");
                }

                goal.UserId = userId;

                // Ensure DateTime values are in UTC
                goal.StartDate = DateTime.SpecifyKind(goal.StartDate, DateTimeKind.Utc);
                goal.EndDate = DateTime.SpecifyKind(goal.EndDate, DateTimeKind.Utc);

                _logger.LogInformation($"Attempting to create goal: {goal.Name} for user ID: {goal.UserId}");

                try
                {
                    await _goalService.CreateGoalAsync(goal);
                    _logger.LogInformation($"Goal '{goal.Name}' created successfully.");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during goal creation.");
                    ModelState.AddModelError("", "An error occurred while creating the goal.");
                }
            }
            else
            {
                _logger.LogWarning("Model state is invalid during goal creation.");
            }

            return View(goal);
        }
    }
}
