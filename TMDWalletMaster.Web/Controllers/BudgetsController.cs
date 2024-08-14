using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;

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

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            var budgets = await _budgetService.GetAllBudgetsAsync();
            return View(budgets);
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }
// GET: Budgets/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Amount,StartDate,EndDate,CategoryId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                await _budgetService.CreateBudgetAsync(budget);
                // Получение текущего пользователя
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return RedirectToAction("Profile", "User", new { id = userId });
            }
            ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name", budget.CategoryId);
            return View(budget);
        }


        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            if (budget == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name", budget.CategoryId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Amount,StartDate,EndDate,CategoryId")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _budgetService.UpdateBudgetAsync(budget);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name", budget.CategoryId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _budgetService.DeleteBudgetAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
