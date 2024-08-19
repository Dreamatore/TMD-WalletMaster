using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMDWalletMaster.Web.ViewModels;

namespace TMDWalletMaster.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IBudgetService _budgetService;
        private readonly IGoalService _goalService;
        private readonly IUserService _userService;
        private readonly IExcelImportService _excelImportService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            ITransactionService transactionService,
            IBudgetService budgetService,
            IGoalService goalService,
            IUserService userService,
            IExcelImportService excelImportService,
            ILogger<ProfileController> logger)
        {
            _transactionService = transactionService;
            _budgetService = budgetService;
            _goalService = goalService;
            _userService = userService;
            _excelImportService = excelImportService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accessing Profile/Index.");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("UserId is not valid. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found. Redirecting to NotFound.", userId);
                return NotFound();
            }

            _logger.LogInformation("User with ID {UserId} found. Retrieving additional data.", userId);

            var viewModel = new UserProfileViewModel
            {
                User = user,
                Budgets = await _budgetService.GetBudgetsByUserIdAsync(userId),
                Transactions = await _transactionService.GetTransactionsByUserIdAsync(userId),
                Goals = await _goalService.GetGoalsByUserIdAsync(userId)
            };

            _logger.LogInformation("Returning View for User with ID {UserId}.", userId);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please upload a valid file.");
                return RedirectToAction("Index");
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                ModelState.AddModelError("", "User is not authenticated.");
                return RedirectToAction("Index");
            }

            try
            {
                using var stream = file.OpenReadStream();
                var bankTransactions = _excelImportService.ImportTransactions(stream);

                foreach (var bankTransaction in bankTransactions)
                {
                    var transaction = new Transaction
                    {
                        UserId = userId,
                        Amount = bankTransaction.Amount,
                        Date = DateTime.SpecifyKind(bankTransaction.TransactionDate, DateTimeKind.Utc), // Ensure DateTime is in UTC
                        Description = bankTransaction.Description,
                        Category = bankTransaction.Category
                    };

                    await _transactionService.CreateTransactionAsync(transaction);
                }

                _logger.LogInformation("Bank data imported successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing bank data.");
                ModelState.AddModelError("", "An error occurred while importing the bank data.");
            }

            return RedirectToAction("Index");
        }

    }
}
