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
                _logger.LogWarning("No file uploaded or file is empty.");
                ModelState.AddModelError("", "Please upload a valid file.");
                return RedirectToAction("Index");
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("User is not authenticated.");
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
                        Date = bankTransaction.TransactionDate.ToUniversalTime(),
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

        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id);
                _logger.LogInformation("Transaction with ID {Id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction with ID {Id}.", id);
                ModelState.AddModelError("", "An error occurred while deleting the transaction.");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearAllTransactions()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("User is not authenticated. Cannot clear transactions.");
                ModelState.AddModelError("", "User is not authenticated.");
                return RedirectToAction("Index");
            }

            try
            {
                await _transactionService.DeleteAllTransactionsByUserIdAsync(userId);
                _logger.LogInformation("All transactions for user ID {UserId} cleared successfully.", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing all transactions for user ID {UserId}.", userId);
                ModelState.AddModelError("", "An error occurred while clearing all transactions.");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction with ID {Id} not found for editing.", id);
                return NotFound();
            }

            var model = new TransactionViewModel
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                Category = transaction.Category
            };

            _logger.LogInformation("Transaction with ID {Id} fetched successfully for editing.", id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TransactionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Model state errors: {Errors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                return View(model);
            }

            var transaction = await _transactionService.GetTransactionByIdAsync(model.Id);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction with ID {Id} not found for update.", model.Id);
                return NotFound();
            }

            transaction.Amount = model.Amount;
            transaction.Date = model.Date.ToUniversalTime();
            transaction.Description = model.Description;
            transaction.Category = model.Category;

            try
            {
                await _transactionService.UpdateTransactionAsync(transaction);
                _logger.LogInformation("Transaction with ID {Id} updated successfully. Redirecting to Index.",
                    model.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction with ID {Id}.", model.Id);
                ModelState.AddModelError("", "An error occurred while updating the transaction.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditTransaction(int id)
        {
            _logger.LogInformation("Fetching transaction with ID {Id} for editing.", id);

            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction with ID {Id} not found.", id);
                return NotFound();
            }

            var viewModel = new EditTransactionViewModel
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Category = transaction.Category
            };

            _logger.LogInformation("Transaction with ID {Id} fetched successfully for editing.", id);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditTransaction(EditTransactionViewModel model)
        {
            _logger.LogInformation("Attempting to update transaction with ID {Id}.", model.Id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Model state errors: {Errors}",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                return View(model);
            }

            try
            {
                var transaction = new Transaction
                {
                    Id = model.Id,
                    Description = model.Description,
                    Amount = model.Amount,
                    Date = model.Date.ToUniversalTime(),
                    Category = model.Category
                };

                var updatedTransaction = await _transactionService.UpdateTransactionAsync(transaction);

                if (updatedTransaction == null)
                {
                    _logger.LogWarning(
                        "Transaction with ID {Id} was not updated. It may not exist or be in the wrong state.",
                        model.Id);
                    return NotFound();
                }

                _logger.LogInformation("Transaction with ID {Id} updated successfully. Redirecting to Index.",
                    model.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction with ID {Id}.", model.Id);
                ModelState.AddModelError("", "An error occurred while updating the transaction.");
                return View(model);
            }
        }
    }
}