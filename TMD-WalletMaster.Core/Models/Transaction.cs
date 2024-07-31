namespace TMD_WalletMaster.Core.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int BudgetId { get; set; } // Foreign key for Budget
    public Budget Budget { get; set; } // Navigation property
}