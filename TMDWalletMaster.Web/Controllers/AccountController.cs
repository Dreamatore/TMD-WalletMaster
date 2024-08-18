using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMDWalletMaster.Web.ViewModels;

namespace TMDWalletMaster.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Rendering Login page.");
            return View(new LoginViewModel());
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("Login attempt for user: {UserName}", model.UserName);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid. Checking if user exists.");

                var user = await _userService.GetUserByUserNameAsync(model.UserName);
                if (user != null)
                {
                    _logger.LogInformation("User found: {UserName}. Verifying password.", model.UserName);

                    if (BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                    {
                        _logger.LogInformation("Password verification succeeded for user: {UserName}.", model.UserName);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };
                        var claimsIdentity =
                            new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                        _logger.LogInformation("User {UserName} signed in successfully.", model.UserName);

                        return RedirectToAction("Index", "Navigation");
                    }
                    else
                    {
                        _logger.LogWarning("Password verification failed for user: {UserName}.", model.UserName);
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                else
                {
                    _logger.LogWarning("User not found: {UserName}.", model.UserName);
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            else
            {
                _logger.LogWarning("Model state is not valid during login attempt.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model state error: {Error}", error.ErrorMessage);
                }
            }

            return View(model);
        }


        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Rendering Register page.");
            return View(new RegisterViewModel());
        }

        // POST: Account/Register
[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Register(RegisterViewModel model)
{
    _logger.LogInformation("Register attempt for user: {UserName}", model.UserName);

    if (ModelState.IsValid)
    {
        _logger.LogInformation("Model state is valid. Checking if user exists.");

        var existingUser = await _userService.GetUserByUserNameAsync(model.UserName);

        if (existingUser == null)
        {
            _logger.LogInformation("User does not exist. Creating new user.");

            var newUser = new User
            {
                UserName = model.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            var result = await _userService.RegisterUserAsync(newUser);

            _logger.LogInformation("Registration result: Succeeded={Succeeded}", result.Succeeded);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserName} registered successfully. Signing in user.", model.UserName);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newUser.UserName),
                    new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                _logger.LogInformation("User {UserName} signed in successfully.", model.UserName);
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                _logger.LogError("User registration failed for {UserName}. Errors: {Errors}", model.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        else
        {
            _logger.LogWarning("Username already taken: {UserName}", model.UserName);
            ModelState.AddModelError("", "Username already taken.");
        }
    }
    else
    {
        _logger.LogWarning("Model state is not valid during registration attempt.");
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            _logger.LogWarning("Model state error: {Error}", error.ErrorMessage);
        }
    }

    return View(model);
}


        // GET: Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User logging out.");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out successfully.");
            return RedirectToAction("Login");
        }
    }
}