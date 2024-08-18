using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMDWalletMaster.Web.Controllers
{
    [Authorize]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            // Логирование начала выполнения метода
            Console.WriteLine("Starting Create method for Transaction.");

            // Проверка аутентификации пользователя
            if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("User is not authenticated. Redirecting to login page.");
                return RedirectToAction("Login", "Account");
            }

            // Установить UserId текущего пользователя
            transaction.UserId = User.Identity.Name;
            Console.WriteLine($"UserId extracted: {transaction.UserId}");

            // Конвертируем дату транзакции в формат UTC
            transaction.Date = DateTime.SpecifyKind(transaction.Date, DateTimeKind.Utc);

            // Логирование свойств модели перед валидацией
            Console.WriteLine("Model properties:");
            Console.WriteLine($"Amount: {transaction.Amount}");
            Console.WriteLine($"Date: {transaction.Date}");
            Console.WriteLine($"Description: {transaction.Description}");
            Console.WriteLine($"Category: {transaction.Category}");
            Console.WriteLine($"UserId: {transaction.UserId}");

            // Исключаем UserId из проверки модели (если это необходимо)
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
                    // Логирование перед вызовом сервиса создания транзакции
                    Console.WriteLine(
                        $"Attempting to create transaction for UserId: {transaction.UserId}, Amount: {transaction.Amount}");

                    await _transactionService.CreateTransactionAsync(transaction);

                    // Логирование успешного завершения операции
                    Console.WriteLine("Transaction creation successful. Redirecting to Index.");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    Console.WriteLine("Exception during transaction creation: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while creating the transaction.");
                }
            }
            else
            {
                // Логирование ошибок модели
                Console.WriteLine("Returning to the view with model errors.");
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