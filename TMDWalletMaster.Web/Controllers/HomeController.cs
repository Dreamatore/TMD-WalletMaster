using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace TMDWalletMaster.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TMDWalletMaster", _configuration["EmailSettings:Username"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Your verification code";
            message.Body = new TextPart("plain")
            {
                Text = $"Your verification code is {code}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            // TODO: Сохраните код в базе данных или кэше, чтобы проверить его позже
            // Сохранение кода и данных пользователя

            // Перенаправляем на страницу подтверждения email
            return RedirectToAction("ConfirmEmail");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // TODO: Реализуйте логику входа пользователя
            // Например, проверьте введенные данные и авторизуйте пользователя

            // После входа перенаправляем на страницу профиля
            return RedirectToAction("Profile");
        }

        public IActionResult ConfirmEmail()
        {
            // Страница для подтверждения email или ввода кода подтверждения
            return View();
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
