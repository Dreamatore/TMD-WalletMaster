using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Добавлено для логирования
using TMD_WalletMaster.Core.Services.Interfaces;
using TMDWalletMaster.Web.ViewModels;

namespace TMDWalletMaster.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IBudgetService _budgetService;
        private readonly IGoalService _goalService;
        private readonly IUserService _userService;
        private readonly ILogger<ProfileController> _logger; // Добавлено для логирования

        public ProfileController(
            ITransactionService transactionService,
            IBudgetService budgetService,
            IGoalService goalService,
            IUserService userService,
            ILogger<ProfileController> logger) // Добавлен параметр для логирования
        {
            _transactionService = transactionService;
            _budgetService = budgetService;
            _goalService = goalService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accessing Profile/Index.");

            var userName = User.Identity?.Name;

            if (userName == null)
            {
                _logger.LogWarning("User is not authenticated. Redirecting to Unauthorized.");
                return Unauthorized();
            }

            _logger.LogInformation("User Identity Name: {UserIdentityName}", userName);

            var user = await _userService.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                _logger.LogWarning("User with Username {UserName} not found. Redirecting to NotFound.", userName);
                return NotFound();
            }

            _logger.LogInformation("User with Username {UserName} found. Retrieving additional data.", userName);

            var viewModel = new UserProfileViewModel
            {
                User = user,
                Budgets = await _budgetService.GetBudgetsByUserIdAsync(user.Id),
                Transactions = await _transactionService.GetTransactionsByUserIdAsync(user.Id),
                Goals = await _goalService.GetAllGoalsAsync()
            };

            _logger.LogInformation("Returning View for User with Username {UserName}.", userName);

            return View(viewModel);
        }
    }
}