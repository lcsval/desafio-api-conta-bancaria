using Desafio.Domain.Requests;
using Desafio.Domain.Requests.Customer;
using Desafio.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desafio.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Result<List<AccountResponse>>> GetAll();
        Task<Result<AccountResponse>> GetById(Guid id);
        Task<Result<AccountResponse>> Create(CreateAccountRequest request);
        //Task<Result<AccountResponse>> Update(Guid id, UpdateAccountRequest request);
        Task<Result<ExtractResponse>> Extract(Guid accountId);
        Task<Result<DepositResponse>> Deposit(DepositRequest request);
    }
}
