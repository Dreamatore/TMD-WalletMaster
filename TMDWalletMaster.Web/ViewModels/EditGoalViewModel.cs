using System;
using System.ComponentModel.DataAnnotations;

namespace TMDWalletMaster.Web.ViewModels
{
    public class EditGoalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the goal.")]
        [StringLength(100, ErrorMessage = "The goal name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a target amount.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than zero.")]
        public decimal TargetAmount { get; set; }

        [Required(ErrorMessage = "Please enter the current amount.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Current amount must be zero or greater.")]
        public decimal CurrentAmount { get; set; }

        [Required(ErrorMessage = "Please enter a start date.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter an end date.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}