using System;
using System.ComponentModel.DataAnnotations;

namespace TMDWalletMaster.Web.ViewModels
{
    public class EditGoalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the goal.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a target amount.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than zero.")]
        public decimal TargetAmount { get; set; }

        [Required(ErrorMessage = "Please enter a start date.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter an end date.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please enter a priority.")]
        [Range(1, 10, ErrorMessage = "Priority must be between 1 and 10.")]
        public int Priority { get; set; }
    }
}