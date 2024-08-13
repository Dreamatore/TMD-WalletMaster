using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Web.Models;
using TMD_WalletMaster.Core.Data; 

namespace TMDWalletMaster.Web.Controllers
{
    public class NavigationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NavigationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new NavigationViewModel
            {
                Budgets = _context.Budgets.ToList(),
                Goals = _context.Goals.ToList(),
                Transactions = _context.Transactions.ToList(),
                Categories = _context.Categories.ToList()
            };

            return View(viewModel);
        }
    }
}