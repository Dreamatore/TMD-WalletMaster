using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMDWalletMaster.Web.ViewModels;


namespace TMDWalletMaster.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;

        public UserController(IUserService userService, IBudgetService budgetService, ITransactionService transactionService)
        {
            _userService = userService;
            _budgetService = budgetService;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Profile(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var budgets = await _budgetService.GetBudgetsByUserIdAsync(id); // Исправлено
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(id); // Исправлено

            var model = new UserProfileViewModel
            {
                User = user,
                Budgets = budgets,
                Transactions = transactions
            };

            return View(model);
        }
    }
}