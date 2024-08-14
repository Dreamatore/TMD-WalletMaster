using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
    }
}