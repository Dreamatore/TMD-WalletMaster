using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMDWalletMaster.Web.ViewModels;
using System.Threading.Tasks;

namespace TMDWalletMaster.Web.Controllers
{
    public class NavigationController : Controller
    {
        private readonly IBudgetService _budgetService;

        public NavigationController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = "1"; // Замените на реальный идентификатор пользователя
            var budgets = await _budgetService.GetBudgetsByUserIdAsync(userId);

            var model = new UserProfileViewModel
            {
                Budgets = budgets
                // Другие данные, если нужно
            };

            return View(model);
        }
    }
}