using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using TMDWalletMaster.Web.ViewModels;

namespace TMDWalletMaster.Web.Controllers
{
    [Authorize] // Убедитесь, что пользователи должны быть аутентифицированы для доступа к этому контроллеру
    public class NavigationController : Controller
    {
        private readonly IBudgetService _budgetService;

        // Внедрение зависимости
        public NavigationController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        // Метод для отображения страницы профиля пользователя
        public async Task<IActionResult> Index()
        {
            // Получаем идентификатор текущего пользователя из Claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                // Если идентификатор пользователя не найден или преобразование не удалось, перенаправляем на страницу входа
                return RedirectToAction("Login", "Account");
            }

            // Получаем бюджеты текущего пользователя
            var budgets = await _budgetService.GetBudgetsByUserIdAsync(userId);

            // Создаём модель для представления
            var model = new UserProfileViewModel
            {
                Budgets = budgets
                // Можно добавить другие данные, если нужно
            };

            // Возвращаем представление с моделью
            return View(model);
        }
    }
}