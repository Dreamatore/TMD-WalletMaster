using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMD_WalletMaster.Core.Services;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Добавление служб в контейнер.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Настройка контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)); // Опционально для улучшения производительности


// Регистрация служб
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IGoalRepository,GoalRepository>();
builder.Services.AddScoped<ITransactionService,TransactionService>();
builder.Services.AddScoped<ITransactionRepository,TransactionRepository>();
// Регистрация конфигурации почтового сервиса
builder.Services.AddSingleton(builder.Configuration);

var app = builder.Build();

// Настройка обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

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
});


app.Run();