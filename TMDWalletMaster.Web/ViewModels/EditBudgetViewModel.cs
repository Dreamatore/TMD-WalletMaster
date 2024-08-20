using System;
using System.ComponentModel.DataAnnotations;

namespace TMDWalletMaster.Web.ViewModels
{
    public class EditBudgetViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the budget.")]
        [StringLength(100, ErrorMessage = "The budget name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an amount.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please enter a start date.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter an end date.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
    }
}