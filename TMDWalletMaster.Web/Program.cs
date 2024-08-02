using Microsoft.EntityFrameworkCore;
using TMDWalletMaster.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавьте строку подключения к DI-контейнеру
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TMDWalletMaster.Core")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройте middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();