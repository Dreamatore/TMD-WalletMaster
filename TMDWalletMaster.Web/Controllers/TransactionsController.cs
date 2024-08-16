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
                // Создание новой транзакции
                await _transactionService.CreateTransactionAsync(transaction);

                // Получение всех транзакций
                var transactions = await _transactionService.GetAllTransactionsAsync();

                // Проверка на null и пустоту
                if (transactions == null)
                {
                    transactions = new List<Transaction>(); // Создание пустого списка, если транзакций нет
                }

                // Логика для аналитики
                var statistics = new
                {
                    TotalAmount = transactions.Sum(t => t.Amount),
                    AverageAmount = transactions.Any() ? transactions.Average(t => t.Amount) : 0,
                    Categories = transactions.GroupBy(t => t.Category)
                        .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
                        .ToList()
                };

                // Передача данных в представление
                ViewBag.Statistics = statistics;

                // Передача коллекции транзакций в представление
                return View("Create", transactions);
            }

            // Если модель невалидна, возвращаем пустой список транзакций в представление
            return View("Create", new List<Transaction>());
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