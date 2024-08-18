using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace TMDWalletMaster.Web.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly IBudgetService _budgetService;
        private readonly ICategoryService _categoryService;

        public BudgetsController(IBudgetService budgetService, ICategoryService categoryService)
        {
            _budgetService = budgetService;
            _categoryService = categoryService;
        }

        // GET: Budgets/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
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
            // Логирование начала выполнения метода
            Console.WriteLine("Starting Create method.");

            // Проверьте, аутентифицирован ли пользователь
            if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("User is not authenticated. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            // Получаем идентификатор текущего пользователя из Claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                Console.WriteLine("UserId is not valid. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            // Установите UserId из текущего пользователя
            budget.UserId = userId;
            Console.WriteLine($"UserId extracted: {budget.UserId}");

            // Конвертируем даты в формат UTC
            budget.StartDate = DateTime.SpecifyKind(budget.StartDate, DateTimeKind.Utc);
            budget.EndDate = DateTime.SpecifyKind(budget.EndDate, DateTimeKind.Utc);

            // Логирование состояния модели перед валидацией
            Console.WriteLine("Model properties:");
            Console.WriteLine($"Name: {budget.Name}");
            Console.WriteLine($"Amount: {budget.Amount}");
            Console.WriteLine($"StartDate: {budget.StartDate}");
            Console.WriteLine($"EndDate: {budget.EndDate}");
            Console.WriteLine($"CategoryId: {budget.CategoryId}");
            Console.WriteLine($"UserId: {budget.UserId}");

            // Исключаем UserId из проверки модели
            ModelState.Remove("UserId");

            // Логирование состояния модели
            Console.WriteLine("Model state is valid: " + ModelState.IsValid);
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid. Errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Логирование перед вызовом сервиса создания бюджета
                    Console.WriteLine($"Attempting to create budget: {budget.Name}, UserId: {budget.UserId}");

                    await _budgetService.CreateBudgetAsync(budget);

                    // Логирование успешного завершения операции
                    Console.WriteLine("Budget creation successful. Redirecting to Index.");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    Console.WriteLine("Exception during budget creation: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while creating the budget.");
                }
            }
            else
            {
                // Логирование ошибок модели
                Console.WriteLine("Returning to the view with model errors.");
                // Перезагружаем категории, если модель невалидна
                try
                {
                    Console.WriteLine("Reloading categories for the ViewBag.");

                    var categories = await _categoryService.GetAllCategoriesAsync();
                    if (categories == null || !categories.Any())
                    {
                        Console.WriteLine("No categories found.");
                    }
                    else
                    {
                        Console.WriteLine($"Categories reloaded. Count: {categories.Count()}");
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
                    Console.WriteLine("Exception during reloading categories: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while loading categories.");
                }
            }

            return View(budget);
        }

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            var budgets = await _budgetService.GetAllBudgetsAsync();
            return View(budgets);
        }
    }
}
