namespace PenChecks_ATM_API.Core.DTOs;
public class AccountBalanceResponse
{
    public int AccountId { get; set; }
    public decimal Balance { get; set; }
}

public class TransferResponse
{
    public string Message { get; set; }
    public bool Success { get; set; }
}