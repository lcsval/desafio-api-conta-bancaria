using Desafio.Domain;
using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Requests;
using Desafio.Domain.Requests.Customer;
using Desafio.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desafio.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public BankController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("GetAllAccounts")]
        public async Task<Result<List<AccountResponse>>> GetAllAccounts()
        {
            var result = await _accountService.GetAll();
            return result;
        }

        [HttpGet("GetAccountById/{id}")]
        public async Task<Result<AccountResponse>> GetAccountById
        (
            [FromRoute] Guid id
        )
        {
            var result = await _accountService.GetById(id);
            return result;
        }

        [HttpPost("CreateAccount")]
        public async Task<Result<AccountResponse>> CreateAccount
        (
            [FromBody] CreateAccountRequest request
        )
        {
            var result = await _accountService.Create(request);
            return result;
        }

        [HttpGet("Extract/{accountId}")]
        public async Task<Result<ExtractResponse>> Extract
        (
            [FromRoute] Guid accountId
        )
        {
            var result = await _accountService.Extract(accountId);
            return result;
        }

        [HttpPost("Deposit")]
        public async Task<Result<DepositResponse>> Deposit
        (
            [FromBody] DepositRequest request
        )
        {
            var result = await _accountService.Deposit(request);
            return result;
        }


        [HttpPost("Withdraw")]
        public async Task<Result<WithdrawResponse>> Withdraw
        (
            [FromBody] WithdrawRequest request
        )
        {
            var result = await _accountService.Withdraw(request);
            return result;
        }

        [HttpPost("Transfer")]
        public async Task<Result<TransferResponse>> Transfer
        (
            [FromBody] TransferRequest request
        )
        {
            var result = await _accountService.Transfer(request);
            return result;
        }
    }
}
