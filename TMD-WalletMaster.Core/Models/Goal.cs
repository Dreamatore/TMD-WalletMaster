namespace TMD_WalletMaster.Core.Models;

public class Goal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime DeadLine { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}