using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Web.Controllers
{
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;

        public GoalsController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        // GET: /Goals
        public async Task<IActionResult> Index()
        {
            var goals = await _goalService.GetAllGoalsAsync();
            return View(goals);
        }

        // GET: /Goals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Goals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Goal goal)
        {
            if (ModelState.IsValid)
            {
                await _goalService.CreateGoalAsync(goal);
                return RedirectToAction("Index");
            }

            return View(goal);
        }
    }
}