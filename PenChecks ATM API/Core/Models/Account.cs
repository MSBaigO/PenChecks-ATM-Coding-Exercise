namespace PenChecks_ATM_API.Core.Models;

public class Account
{
    public int Id { get; set; }
    public decimal Balance { get; set; } = 0m;
}