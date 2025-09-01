using PenChecks_ATM_API.Core.Interfaces;
using PenChecks_ATM_API.Core.Models;
using PenChecks_ATM_API.Core.DTOs;

namespace PenChecks_ATM_API.Core.Services;

/// <summary>
/// ATM Service implementing banking operations
/// </summary>
public class ATMService : IATMService
{
    private readonly Dictionary<int, Account> _accounts = [];
    private readonly List<Transaction> _transactions = [];

    /// <summary>
    /// Initializes the ATM service with sample accounts
    /// </summary>
    public ATMService()
    {
        // Initialize with 2 sample accounts
        _accounts[1] = new Account { Id = 1, Balance = 1000m };
        _accounts[2] = new Account { Id = 2, Balance = 500m };
    }

    /// <summary>
    /// Deposits money into an account using the deposit request
    /// </summary>
    /// <param name="request">Deposit request containing account ID and amount</param>
    /// <returns>Updated account balance</returns>
    public decimal Deposit(DepositRequest request)
    {
        // Validate request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be positive");
        }

        var account = GetAccount(request.AccountId);
        account.Balance += request.Amount;

        // Record the transaction
        _transactions.Add(new Transaction
        {
            Timestamp = DateTime.Now,
            Type = TransactionType.Deposit,
            Amount = request.Amount,
            ToAccount = request.AccountId
        });

        return account.Balance;
    }

    /// <summary>
    /// Withdraws money from an account using the withdraw request
    /// </summary>
    /// <param name="request">Withdraw request containing account ID and amount</param>
    /// <returns>Updated account balance</returns>
    public decimal Withdraw(WithdrawRequest request)
    {
        // Validate request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be positive");
        }

        var account = GetAccount(request.AccountId);

        if (account.Balance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient funds");
        }

        account.Balance -= request.Amount;

        // Record the transaction
        _transactions.Add(new Transaction
        {
            Timestamp = DateTime.Now,
            Type = TransactionType.Withdraw,
            Amount = request.Amount,
            FromAccount = request.AccountId
        });

        return account.Balance;
    }

    /// <summary>
    /// Transfers money between accounts using the transfer request
    /// </summary>
    /// <param name="request">Transfer request containing source account, destination account, and amount</param>
    public void Transfer(TransferRequest request)
    {
        // Validate request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.FromAccountId == request.ToAccountId)
        {
            throw new ArgumentException("Cannot transfer to the same account");
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be positive");
        }

        // Verify both accounts exist before processing
        var fromAccount = GetAccount(request.FromAccountId);
        var toAccount = GetAccount(request.ToAccountId);

        // Check sufficient funds
        if (fromAccount.Balance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient funds for transfer");
        }

        // Process the transfer
        fromAccount.Balance -= request.Amount;
        toAccount.Balance += request.Amount;

        // Record the transfer transaction
        _transactions.Add(new Transaction
        {
            Timestamp = DateTime.Now,
            Type = TransactionType.Transfer,
            Amount = request.Amount,
            FromAccount = request.FromAccountId,
            ToAccount = request.ToAccountId
        });
    }

    /// <summary>
    /// Gets all transactions
    /// </summary>
    /// <returns>List of all transactions</returns>
    public List<Transaction> GetTransactions() => _transactions;

    /// <summary>
    /// Gets account information by account ID
    /// </summary>
    /// <param name="accountId">The account ID to retrieve</param>
    /// <returns>Account information</returns>
    /// <exception cref="KeyNotFoundException">Thrown when account is not found</exception>
    public Account GetAccount(int accountId)
    {
        if (!_accounts.TryGetValue(accountId, out Account? value))
        {
            throw new KeyNotFoundException($"Account with ID {accountId} not found");
        }
        return value;
    }
}