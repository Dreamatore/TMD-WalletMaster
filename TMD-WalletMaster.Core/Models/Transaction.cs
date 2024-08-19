using System.ComponentModel.DataAnnotations;

namespace TMD_WalletMaster.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required] // Обязательное поле
        public decimal Amount { get; set; }

        [Required] // Обязательное поле
        public DateTime Date { get; set; }

        [Required] // Обязательное поле
        public string Description { get; set; }

        [Required] // Обязательное поле
        public string Category { get; set; }
    }
}