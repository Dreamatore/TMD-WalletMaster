using System.Collections.Generic;
using System.Threading.Tasks;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Data;
using TMD_WalletMaster.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TMD_WalletMaster.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}