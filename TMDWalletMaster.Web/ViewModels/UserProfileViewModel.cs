using System.Collections.Generic;
using TMD_WalletMaster.Core.Models;

namespace TMDWalletMaster.Web.ViewModels
{
    public class UserProfileViewModel
    {
        // Данные пользователя, если они нужны в представлении
        public User User { get; set; } 

        // Коллекция бюджетов пользователя
        public IEnumerable<Budget> Budgets { get; set; }

        // Коллекция транзакций пользователя, если они нужны в представлении
        public IEnumerable<Transaction> Transactions { get; set; }

        // Конструктор для инициализации коллекций
        public UserProfileViewModel()
        {
            Budgets = new List<Budget>();
            Transactions = new List<Transaction>();
        }
    }
}