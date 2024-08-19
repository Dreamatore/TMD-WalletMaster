using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMD_WalletMaster.Core.Services;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 

// Добавление служб в контейнер
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
    .AddRazorRuntimeCompilation();

// Настройка контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)); 

// Регистрация служб
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExcelImportService, ExcelImportService>();

// Регистрация конфигурации почтового сервиса
builder.Services.AddSingleton(builder.Configuration);

// Настройка конфигурации куки и аутентификации
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();


builder.Services.AddAuthorization();

var app = builder.Build();

// Настройка обработки запросов
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");

    endpoints.MapControllerRoute(
        name: "navigation",
        pattern: "Navigation/{action=Index}/{id?}",
        defaults: new { controller = "Navigation" });

    endpoints.MapControllerRoute(
        name: "transactions",
        pattern: "Transactions/{action=Index}/{id?}",
        defaults: new { controller = "Transactions" });

    endpoints.MapControllerRoute(
        name: "goals",
        pattern: "Goals/{action=Index}/{id?}",
        defaults: new { controller = "Goals", action = "Index" });

    endpoints.MapControllerRoute(
        name: "budgets",
        pattern: "Budgets/{action=Index}/{id?}",
        defaults: new { controller = "Budgets", action = "Index" });

    endpoints.MapControllers(); 
});

app.Run();
