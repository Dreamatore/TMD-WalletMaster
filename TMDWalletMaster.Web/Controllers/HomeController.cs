using Microsoft.AspNetCore.Mvc;

namespace TMDWalletMaster.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Budgets/Index.cshtml");
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