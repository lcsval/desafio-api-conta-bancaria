using Desafio.Domain;
using Desafio.Domain.Interfaces.Services;
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

        [HttpGet("GetAll")]
        public async Task<Result<List<AccountResponse>>> GetAll()
        {
            var result = await _accountService.GetAll();
            return result;
        }

        [HttpGet("GetById/{id}")]
        public async Task<Result<AccountResponse>> GetById
        (
            [FromRoute] Guid id
        )
        {
            var result = await _accountService.GetById(id);
            return result;
        }

        [HttpPost("Create")]
        public async Task<Result<AccountResponse>> Create
        (
            [FromBody] CreateAccountRequest request
        )
        {
            var result = await _accountService.Create(request);
            return result;
        }

        //[HttpPut("Update/{id}")]
        //public async Task<Result<AccountResponse>> Update
        //(
        //    [FromRoute] Guid id,
        //    [FromBody] UpdateAccountRequest request
        //)
        //{
        //    var result = await _accountService.Update(id, request);
        //    return result;
        //}
    }
}
