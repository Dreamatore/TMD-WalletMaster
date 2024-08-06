using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Services.Interfaces;
using TMD_WalletMaster.Core.Services;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using TMD_WalletMaster.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Добавление служб в контейнер.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Настройка контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация служб
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Budgets}/{action=Index}/{id?}");

app.Run();