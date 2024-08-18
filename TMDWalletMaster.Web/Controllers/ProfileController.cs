using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMDWalletMaster.Web.ViewModels;

namespace TMDWalletMaster.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IBudgetService _budgetService;
        private readonly IGoalService _goalService;

        public ProfileController(ITransactionService transactionService, IBudgetService budgetService, IGoalService goalService)
        {
            _transactionService = transactionService;
            _budgetService = budgetService;
            _goalService = goalService;
        }

        public async Task<IActionResult> Index()
        {
            // Замените "userId" на реальный ID текущего пользователя
            var userId = User.Identity?.Name; // предполагаем, что имя пользователя используется как его ID

            if (userId == null)
            {
                return Unauthorized();
            }

            var viewModel = new UserProfileViewModel
            {
                User = new User { Id = userId, Name = "User Name" }, // Замените на реальное получение данных пользователя
                Budgets = await _budgetService.GetBudgetsByUserIdAsync(userId),
                Transactions = await _transactionService.GetTransactionsByUserIdAsync(userId),
                Goals = await _goalService.GetAllGoalsAsync() // если нужно только для текущего пользователя, измените метод
            };

            return View(viewModel);
        }
    }
}