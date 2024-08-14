namespace TMD_WalletMaster.Core.Models;

public class Budget
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Transaction> Transactions { get; set; } 
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public string UserId { get; set; }
}