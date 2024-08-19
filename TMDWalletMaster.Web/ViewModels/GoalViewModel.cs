using System;
using System.ComponentModel.DataAnnotations;

namespace TMDWalletMaster.Web.ViewModels
{
    public class GoalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The goal name is required.")]
        [StringLength(100, ErrorMessage = "The goal name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The target amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The target amount must be greater than zero.")]
        public decimal TargetAmount { get; set; }

        [Required(ErrorMessage = "The current amount is required.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "The current amount must be zero or greater.")]
        public decimal CurrentAmount { get; set; }

        [Required(ErrorMessage = "The start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The end date is required.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}