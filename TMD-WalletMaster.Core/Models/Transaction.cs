using System.ComponentModel.DataAnnotations;

namespace TMD_WalletMaster.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required] 
        public DateTime Date { get; set; }

        [Required] 
        public string Description { get; set; }

        [Required] 
        public string Category { get; set; }
    }
}