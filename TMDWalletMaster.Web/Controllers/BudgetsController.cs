﻿using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TMDWalletMaster.Web.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
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
                return NotFound(); // Отправляет пользователя на страницу ошибки 404
            }
            return View(budget); // Убедитесь, что View находится в папке Views/Budgets/Details.cshtml
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            return View(); // Убедитесь, что View находится в папке Views/Budgets/Create.cshtml
        }

        // POST: Budgets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Amount,StartDate,EndDate")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                await _budgetService.CreateBudgetAsync(budget);
                return RedirectToAction(nameof(Index));
            }
            return View(budget); // Убедитесь, что View находится в папке Views/Budgets/Create.cshtml
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            if (budget == null)
            {
                return NotFound(); // Отправляет пользователя на страницу ошибки 404
            }
            return View(budget); // Убедитесь, что View находится в папке Views/Budgets/Edit.cshtml
        }

        // POST: Budgets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Amount,StartDate,EndDate")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound(); // Отправляет пользователя на страницу ошибки 404
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _budgetService.UpdateBudgetAsync(budget);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BudgetExists(budget.Id))
                    {
                        return NotFound(); // Отправляет пользователя на страницу ошибки 404
                    }
                    else
                    {
                        throw; // Перебрасывает исключение для отладки
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(budget); // Убедитесь, что View находится в папке Views/Budgets/Edit.cshtml
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            if (budget == null)
            {
                return NotFound(); // Отправляет пользователя на страницу ошибки 404
            }
            return View(budget); // Убедитесь, что View находится в папке Views/Budgets/Delete.cshtml
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _budgetService.DeleteBudgetAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BudgetExists(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            return budget != null;
        }
    }
}
