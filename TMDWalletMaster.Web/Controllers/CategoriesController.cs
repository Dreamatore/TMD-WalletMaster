using Microsoft.AspNetCore.Mvc;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace TMD_WalletMaster.Web.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("UserId is not valid. Redirecting to login page.");
                throw new UnauthorizedAccessException("Invalid user ID.");
            }
            return userId;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation($"Fetching categories for user with ID: {userId}");

            var categories = await _categoryService.GetCategoriesByUserIdAsync(userId);
            _logger.LogInformation($"Fetched {categories.Count()} categories for user with ID: {userId}");

            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetCurrentUserId();
            var category = await _categoryService.GetCategoryByIdAsync(id, userId);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found.");
                return NotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = GetCurrentUserId();
                    category.UserId = userId;
                    _logger.LogInformation($"Attempting to create category: {category.Name}");
                    await _categoryService.CreateCategoryAsync(category);
                    _logger.LogInformation($"Category '{category.Name}' created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during category creation.");
                    ModelState.AddModelError("", "An error occurred while creating the category.");
                }
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            var category = await _categoryService.GetCategoryByIdAsync(id, userId);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found.");
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                _logger.LogWarning($"Category ID mismatch: URL ID {id} does not match form ID {category.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = GetCurrentUserId();
                    category.UserId = userId;
                    _logger.LogInformation($"Attempting to update category ID {id} with name {category.Name}");
                    await _categoryService.UpdateCategoryAsync(category);
                    _logger.LogInformation($"Category ID {id} updated successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during category update.");
                    ModelState.AddModelError("", "An error occurred while updating the category.");
                }
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            var category = await _categoryService.GetCategoryByIdAsync(id, userId);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found.");
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"Attempting to delete category ID {id}");
                await _categoryService.DeleteCategoryAsync(id, userId);
                _logger.LogInformation($"Category ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during category deletion.");
                ModelState.AddModelError("", "An error occurred while deleting the category.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
