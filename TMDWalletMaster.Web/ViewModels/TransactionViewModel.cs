namespace TMDWalletMaster.Web.ViewModels;
public class TransactionViewModel
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}