using System.Collections.Generic;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Models
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public IEnumerable<Budget> Budgets { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}