using Microsoft.AspNetCore.Mvc;
using PenChecks_ATM_API.Core.Interfaces;
using PenChecks_ATM_API.Core.DTOs;

namespace PenChecks_ATM_API.Controllers;

/// <summary>
/// ATM Controller providing banking operations such as deposit, withdraw, and transfer
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ATMController : ControllerBase
{
    private readonly IATMService _atmService;

    /// <summary>
    /// Initializes a new instance of the ATMController
    /// </summary>
    /// <param name="atmService">The ATM service for handling banking operations</param>
    public ATMController(IATMService atmService)
    {
        _atmService = atmService;
    }

    /// <summary>
    /// Deposits money into a specified account
    /// </summary>
    /// <param name="request">Deposit request containing account ID and amount</param>
    /// <returns>Updated account balance information</returns>
    [HttpPost("deposit")]
    public IActionResult Deposit([FromBody] DepositRequest request)
    {
        try
        {
            // Validate request object
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            // Process the deposit using the request DTO
            var balance = _atmService.Deposit(request);

            // Return success response with updated balance
            return Ok(new AccountBalanceResponse
            {
                AccountId = request.AccountId,
                Balance = balance
            });
        }
        catch (Exception ex)
        {
            // Return error message if deposit fails
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Withdraws money from a specified account
    /// </summary>
    /// <param name="request">Withdraw request containing account ID and amount</param>
    /// <returns>Updated account balance information</returns>
    [HttpPost("withdraw")]
    public IActionResult Withdraw([FromBody] WithdrawRequest request)
    {
        try
        {
            // Validate request object
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            // Process the withdrawal using the request DTO
            var balance = _atmService.Withdraw(request);

            // Return success response with updated balance
            return Ok(new AccountBalanceResponse
            {
                AccountId = request.AccountId,
                Balance = balance
            });
        }
        catch (Exception ex)
        {
            // Return error message if withdrawal fails
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Transfers money from one account to another
    /// </summary>
    /// <param name="request">Transfer request containing source account, destination account, and amount</param>
    /// <returns>Transfer operation result</returns>
    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {
        try
        {
            // Validate request object
            if (request == null)
            {
                return BadRequest(new TransferResponse
                {
                    Message = "Request cannot be null",
                    Success = false
                });
            }

            // Process the transfer using the request DTO
            _atmService.Transfer(request);

            // Return success response
            return Ok(new TransferResponse
            {
                Message = "Transfer successful",
                Success = true
            });
        }
        catch (Exception ex)
        {
            // Return structured error response if transfer fails
            return BadRequest(new TransferResponse
            {
                Message = ex.Message,
                Success = false
            });
        }
    }

    /// <summary>
    /// Retrieves all transaction history
    /// </summary>
    /// <returns>List of all transactions</returns>
    [HttpGet("transactions")]
    public IActionResult GetTransactions()
    {
        try
        {
            var transactions = _atmService.GetTransactions();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves account information by account ID
    /// </summary>
    /// <param name="id">The account ID to retrieve</param>
    /// <returns>Account information</returns>
    [HttpGet("account/{id}")]
    public IActionResult GetAccount(int id)
    {
        try
        {
            var account = _atmService.GetAccount(id);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}