using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMDWalletMaster.Web.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly IBudgetService _budgetService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<BudgetsController> _logger;

        public BudgetsController(IBudgetService budgetService, ICategoryService categoryService, ILogger<BudgetsController> logger)
        {
            _budgetService = budgetService;
            _categoryService = categoryService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("UserId is not valid. Redirecting to login page.");
                throw new UnauthorizedAccessException("Invalid user ID.");
            }
            return userId;
        }


        // GET: Budgets/Create
        public async Task<IActionResult> Create()
        {
            var userId = GetCurrentUserId(); // Получаем текущий UserId
            var categories = await _categoryService.GetCategoriesByUserIdAsync(userId); // Обновляем вызов метода
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View();
        }

        // POST: Budgets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Budget budget)
        {
            _logger.LogInformation("Starting Create method.");

            // Проверьте, аутентифицирован ли пользователь
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is not authenticated. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            // Получаем идентификатор текущего пользователя из Claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("UserId is not valid. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            // Установите UserId из текущего пользователя
            budget.UserId = userId;
            _logger.LogInformation($"UserId extracted: {budget.UserId}");

            // Конвертируем даты в формат UTC
            budget.StartDate = DateTime.SpecifyKind(budget.StartDate, DateTimeKind.Utc);
            budget.EndDate = DateTime.SpecifyKind(budget.EndDate, DateTimeKind.Utc);

            // Логирование состояния модели перед валидацией
            _logger.LogInformation("Model properties:");
            _logger.LogInformation($"Name: {budget.Name}");
            _logger.LogInformation($"Amount: {budget.Amount}");
            _logger.LogInformation($"StartDate: {budget.StartDate}");
            _logger.LogInformation($"EndDate: {budget.EndDate}");
            _logger.LogInformation($"CategoryId: {budget.CategoryId}");
            _logger.LogInformation($"UserId: {budget.UserId}");

            // Исключаем UserId из проверки модели
            ModelState.Remove("UserId");

            // Логирование состояния модели
            _logger.LogInformation("Model state is valid: {IsValid}", ModelState.IsValid);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Логирование перед вызовом сервиса создания бюджета
                    _logger.LogInformation("Attempting to create budget: {Name}, UserId: {UserId}", budget.Name, budget.UserId);

                    await _budgetService.CreateBudgetAsync(budget);

                    // Логирование успешного завершения операции
                    _logger.LogInformation("Budget creation successful. Redirecting to Index.");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    _logger.LogError(ex, "Exception during budget creation.");
                    ModelState.AddModelError("", "An error occurred while creating the budget.");
                }
            }
            else
            {
                // Логирование ошибок модели
                _logger.LogWarning("Returning to the view with model errors.");
                // Перезагружаем категории, если модель невалидна
                try
                {
                    _logger.LogInformation("Reloading categories for the ViewBag.");

                    var categories = await _categoryService.GetCategoriesByUserIdAsync(GetCurrentUserId());
                    if (categories == null || !categories.Any())
                    {
                        _logger.LogWarning("No categories found.");
                    }
                    else
                    {
                        _logger.LogInformation("Categories reloaded. Count: {Count}", categories.Count());
                    }

                    ViewBag.Categories = categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                }
                catch (Exception ex)
                {
                    // Логирование ошибки при загрузке категорий
                    _logger.LogError(ex, "Exception during reloading categories.");
                    ModelState.AddModelError("", "An error occurred while loading categories.");
                }
            }

            return View(budget);
        }

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            // Проверяем аутентификацию пользователя
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

            _logger.LogInformation("Fetching budgets for user with ID: {UserId}", userId);

            var budgets = await _budgetService.GetBudgetsByUserIdAsync(userId);
            _logger.LogInformation("Fetched {Count} budgets for user with ID: {UserId}", budgets.Count(), userId);

            return View(budgets);
        }
        
    }
}
