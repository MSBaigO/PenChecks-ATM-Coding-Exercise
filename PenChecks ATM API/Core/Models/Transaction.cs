namespace PenChecks_ATM_API.Core.Models;

public class Transaction
{
    public DateTime Timestamp { get; set; }
    public TransactionType Type { get; set; } 
    public decimal Amount { get; set; }
    public int? FromAccount { get; set; }
    public int? ToAccount { get; set; }
}

public enum TransactionType
{
    Deposit,
    Withdraw,
    Transfer
}