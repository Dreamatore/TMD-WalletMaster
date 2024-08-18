// IUserRepository.cs
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id); // Используйте int
        Task<User> GetUserByUserNameAsync(string userName);
        Task AddAsync(User user);
    }
}