using PenChecks_ATM_API.Core.DTOs;
using PenChecks_ATM_API.Core.Services;
using PenChecks_ATM_API.Core.Models;

namespace ATMApp.Tests;

/// <summary>
/// Unit tests for ATMService using DTO-based methods
/// </summary>
public class ATMServiceTests
{
    private readonly ATMService _service;

    public ATMServiceTests()
    {
        _service = new ATMService();
    }

    [Fact]
    public void Deposit_ShouldIncreaseBalance_WhenValidRequest()
    {
        // Arrange
        var depositRequest = new DepositRequest
        {
            AccountId = 1,
            Amount = 100m
        };

        // Act
        var balance = _service.Deposit(depositRequest);

        // Assert
        Assert.Equal(1100m, balance);
    }

    [Fact]
    public void Deposit_ShouldThrow_WhenRequestIsNull()
    {
        // Arrange
        DepositRequest request = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.Deposit(request));
    }

    [Fact]
    public void Deposit_ShouldThrow_WhenAmountIsZeroOrNegative()
    {
        // Arrange
        var depositRequest = new DepositRequest
        {
            AccountId = 1,
            Amount = -50m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.Deposit(depositRequest));
    }

    [Fact]
    public void Deposit_ShouldThrow_WhenAccountNotFound()
    {
        // Arrange
        var depositRequest = new DepositRequest
        {
            AccountId = 999, // Non-existent account
            Amount = 100m
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _service.Deposit(depositRequest));
    }

    [Fact]
    public void Withdraw_ShouldDecreaseBalance_WhenValidRequest()
    {
        // Arrange
        var withdrawRequest = new WithdrawRequest
        {
            AccountId = 1,
            Amount = 200m
        };

        // Act
        var balance = _service.Withdraw(withdrawRequest);

        // Assert
        Assert.Equal(800m, balance);
    }

    [Fact]
    public void Withdraw_ShouldThrow_WhenRequestIsNull()
    {
        // Arrange
        WithdrawRequest request = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.Withdraw(request));
    }

    [Fact]
    public void Withdraw_ShouldThrow_WhenAmountIsZeroOrNegative()
    {
        // Arrange
        var withdrawRequest = new WithdrawRequest
        {
            AccountId = 1,
            Amount = 0m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.Withdraw(withdrawRequest));
    }

    [Fact]
    public void Withdraw_ShouldThrow_WhenInsufficientFunds()
    {
        // Arrange
        var withdrawRequest = new WithdrawRequest
        {
            AccountId = 2, // Account with 500m balance
            Amount = 10000m
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.Withdraw(withdrawRequest));
    }

    [Fact]
    public void Withdraw_ShouldThrow_WhenAccountNotFound()
    {
        // Arrange
        var withdrawRequest = new WithdrawRequest
        {
            AccountId = 999, // Non-existent account
            Amount = 100m
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _service.Withdraw(withdrawRequest));
    }

    [Fact]
    public void Transfer_ShouldMoveFundsBetweenAccounts_WhenValidRequest()
    {
        // Arrange
        var transferRequest = new TransferRequest
        {
            FromAccountId = 1,
            ToAccountId = 2,
            Amount = 300m
        };

        var fromBefore = _service.GetAccount(1).Balance;
        var toBefore = _service.GetAccount(2).Balance;

        // Act
        _service.Transfer(transferRequest);

        // Assert
        var fromAfter = _service.GetAccount(1).Balance;
        var toAfter = _service.GetAccount(2).Balance;

        Assert.Equal(fromBefore - 300m, fromAfter);
        Assert.Equal(toBefore + 300m, toAfter);
    }

    [Fact]
    public void Transfer_ShouldThrow_WhenRequestIsNull()
    {
        // Arrange
        TransferRequest request = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.Transfer(request));
    }

    [Fact]
    public void Transfer_ShouldThrow_WhenSameAccount()
    {
        // Arrange
        var transferRequest = new TransferRequest
        {
            FromAccountId = 1,
            ToAccountId = 1, // Same account
            Amount = 50m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.Transfer(transferRequest));
    }

    [Fact]
    public void Transfer_ShouldThrow_WhenAmountIsZeroOrNegative()
    {
        // Arrange
        var transferRequest = new TransferRequest
        {
            FromAccountId = 1,
            ToAccountId = 2,
            Amount = -100m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.Transfer(transferRequest));
    }

    [Fact]
    public void Transfer_ShouldThrow_WhenInsufficientFunds()
    {
        // Arrange
        var transferRequest = new TransferRequest
        {
            FromAccountId = 2, // Account with 500m balance
            ToAccountId = 1,
            Amount = 10000m
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.Transfer(transferRequest));
    }

    [Fact]
    public void Transfer_ShouldThrow_WhenFromAccountNotFound()
    {
        // Arrange
        var transferRequest = new TransferRequest
        {
            FromAccountId = 999, // Non-existent account
            ToAccountId = 1,
            Amount = 100m
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _service.Transfer(transferRequest));
    }

    [Fact]
    public void Transfer_ShouldThrow_WhenToAccountNotFound()
    {
        // Arrange
        var transferRequest = new TransferRequest
        {
            FromAccountId = 1,
            ToAccountId = 999, // Non-existent account
            Amount = 100m
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _service.Transfer(transferRequest));
    }

    [Fact]
    public void GetAccount_ShouldReturnAccount_WhenAccountExists()
    {
        // Act
        var account = _service.GetAccount(1);

        // Assert
        Assert.NotNull(account);
        Assert.Equal(1, account.Id);
        Assert.Equal(1000m, account.Balance);
    }

    [Fact]
    public void GetAccount_ShouldThrow_WhenAccountNotFound()
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _service.GetAccount(999));
    }

    [Fact]
    public void GetTransactions_ShouldReturnEmptyList_Initially()
    {
        // Act
        var transactions = _service.GetTransactions();

        // Assert
        Assert.NotNull(transactions);
        Assert.Empty(transactions);
    }

    [Fact]
    public void GetTransactions_ShouldReturnTransactions_AfterOperations()
    {
        // Arrange
        var depositRequest = new DepositRequest { AccountId = 1, Amount = 100m };
        var withdrawRequest = new WithdrawRequest { AccountId = 1, Amount = 50m };

        // Act
        _service.Deposit(depositRequest);
        _service.Withdraw(withdrawRequest);
        var transactions = _service.GetTransactions();

        // Assert
        Assert.NotNull(transactions);
        Assert.Equal(2, transactions.Count);
        Assert.Contains(transactions, t => t.Type == TransactionType.Deposit);
        Assert.Contains(transactions, t => t.Type == TransactionType.Withdraw);
    }

    [Fact]
    public void MultipleOperations_ShouldMaintainCorrectBalances()
    {
        // Arrange
        var deposit1 = new DepositRequest { AccountId = 1, Amount = 200m };
        var withdraw1 = new WithdrawRequest { AccountId = 1, Amount = 50m };
        var transfer1 = new TransferRequest { FromAccountId = 1, ToAccountId = 2, Amount = 100m };

        // Act
        _service.Deposit(deposit1);        // Account 1: 1000 + 200 = 1200
        _service.Withdraw(withdraw1);      // Account 1: 1200 - 50 = 1150
        _service.Transfer(transfer1);      // Account 1: 1150 - 100 = 1050, Account 2: 500 + 100 = 600

        // Assert
        Assert.Equal(1050m, _service.GetAccount(1).Balance);
        Assert.Equal(600m, _service.GetAccount(2).Balance);
    }
}