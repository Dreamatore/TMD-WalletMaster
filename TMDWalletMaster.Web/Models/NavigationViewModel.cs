
using TMD_WalletMaster.Core.Models;
using System.Collections.Generic;

namespace TMD_WalletMaster.Web.Models
{
    public class NavigationViewModel
    {
        public IEnumerable<Budget> Budgets { get; set; }
        public IEnumerable<Goal> Goals { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}