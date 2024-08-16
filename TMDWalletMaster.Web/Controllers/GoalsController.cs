using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMDWalletMaster.Web.Controllers
{
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;

        public GoalsController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public async Task<IActionResult> Index()
        {
            var goals = await _goalService.GetAllGoalsAsync();
            return View(goals);
        }
    }
}