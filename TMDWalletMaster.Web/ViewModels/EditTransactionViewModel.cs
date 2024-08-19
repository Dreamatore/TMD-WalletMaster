using System;
using System.ComponentModel.DataAnnotations;

namespace TMDWalletMaster.Web.ViewModels
{
    public class EditTransactionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Category { get; set; } 
    }
}