using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Web.Controllers
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
            if (ModelState.IsValid)
            {
                try
                {
                    await _budgetService.CreateBudgetAsync(budget);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the budget: " + ex.Message);
                }
            }
            // Reload categories if model state is not valid
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
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
