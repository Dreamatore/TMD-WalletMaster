using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}