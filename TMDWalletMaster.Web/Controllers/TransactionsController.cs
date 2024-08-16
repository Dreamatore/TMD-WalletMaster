using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMDWalletMaster.Web.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            // Получение всех транзакций для пользователя
            var userId = User.Identity.Name; // Или другой способ получения ID пользователя
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(userId);
            var totalAmount = await _transactionService.GetTotalAmountByUserIdAsync(userId);

            ViewBag.TotalAmount = totalAmount; // Передача суммы в представление

            return View(transactions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.CreateTransactionAsync(transaction);

                // Дополнительная логика для аналитики
                var transactions = await _transactionService.GetAllTransactionsAsync();
                var statistics = new
                {
                    TotalAmount = transactions.Sum(t => t.Amount),
                    AverageAmount = transactions.Average(t => t.Amount),
                    Categories = transactions.GroupBy(t => t.Category)
                        .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
                        .ToList()
                };

                ViewBag.Statistics = statistics;
                return View("Create", transaction);
            }
            return View(transaction);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.UpdateTransactionAsync(transaction);
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return RedirectToAction("Index");
        }
    }
}