using PenChecks_ATM_API.Core.DTOs;
using PenChecks_ATM_API.Core.Models;

namespace PenChecks_ATM_API.Core.Interfaces
{
    public interface IATMService
    {
        decimal Deposit(DepositRequest request);
        decimal Withdraw(WithdrawRequest request);
        void Transfer(TransferRequest request);
        List<Transaction> GetTransactions();
        Account GetAccount(int accountId);
    }
}
