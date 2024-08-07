using Microsoft.AspNetCore.Mvc;
using FluentEmail.Core;
using FluentEmail.MailKitSmtp;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace TMDWalletMaster.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFluentEmail _emailSender;
        private readonly IConfiguration _configuration;

        public HomeController(IFluentEmail emailSender, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            // Отображаем страницу регистрации и входа
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string username, string password)
        {
            // Генерация уникального кода
            var code = GenerateVerificationCode();

            // Отправка email
            var response = await _emailSender
                .To(email)
                .Subject("Your verification code")
                .Body($"Your verification code is {code}")
                .SendAsync();

            if (response.Successful)
            {
                // Сохраните код в базе данных или кэше, чтобы проверить его позже
                // TODO: Реализуйте сохранение кода и данных пользователя

                // Перенаправляем на страницу подтверждения email
                return RedirectToAction("ConfirmEmail");
            }

            // Обработка ошибок
            ModelState.AddModelError("", "Failed to send verification email.");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // TODO: Реализуйте логику входа пользователя
            // Например, проверьте введенные данные и авторизуйте пользователя

            // После входа перенаправляем на страницу профиля
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult ConfirmEmail(string emailCode)
        {
            // TODO: Реализуйте проверку кода подтверждения
            // Например, проверьте введенный код и активируйте пользователя

            // После подтверждения перенаправляем на страницу профиля
            return RedirectToAction("Profile");
        }

        public IActionResult Profile()
        {
            // Страница профиля пользователя
            return View();
        }

        // Генерация уникального кода для подтверждения email
        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        // Для демонстрации и проверки, добавим методы для действия Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View("~/Views/Budgets/Create.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Views/Budgets/Edit.cshtml");
        }

        public IActionResult Details()
        {
            return View("~/Views/Budgets/Details.cshtml");
        }

        public IActionResult Delete()
        {
            return View("~/Views/Budgets/Delete.cshtml");
        }
    }
}
