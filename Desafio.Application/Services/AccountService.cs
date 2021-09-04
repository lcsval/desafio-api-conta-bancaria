using Desafio.Application.Validators.Account;
using Desafio.Domain;
using Desafio.Domain.Entities;
using Desafio.Domain.Enums;
using Desafio.Domain.Interfaces;
using Desafio.Domain.Interfaces.Repositories;
using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Requests.Customer;
using Desafio.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRecordRepository _accountRecordRepository;

        public AccountService
        (
            IUnitOfWork unitOfWork,
            IAccountRepository accountRepository,
            IAccountRecordRepository accountRecordRepository
        )
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
            _accountRecordRepository = accountRecordRepository;
        }

        public async Task<Result<AccountResponse>> Create(CreateAccountRequest request)
        {
            try
            {
                var validator = new CreateAccountValidator();
                var results = validator.Validate(request);
                if (!results.IsValid)
                    return Result<AccountResponse>.Failure(results.Errors.Select(s => s.ErrorMessage));

                _unitOfWork.BeginTransaction();
                var insertedAccount = await _accountRepository.Create(new Account
                {
                    Name = request.Name,
                    Balance = request.Balance
                });

                var insertedAccRecord = await _accountRecordRepository.Create(new AccountRecord
                {
                    AccountId = insertedAccount.Id,
                    Value = request.Balance,
                    Type = AccountTypeEnum.Credito,
                });
                _unitOfWork.Commit();

                return Result<AccountResponse>.Success(MapResponse(insertedAccount));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return Result<AccountResponse>.Failure(new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<AccountResponse>>> GetAll()
        {
            try
            {
                var result = await _accountRepository.GetAll();
                return Result<List<AccountResponse>>.Success(MapResponseList(result));
            }
            catch (Exception ex)
            {
                return Result<List<AccountResponse>>.Failure(new List<string> { ex.Message });
            }
        }

        public async Task<Result<AccountResponse>> GetById(Guid id)
        {
            try
            {
                var validator = new GetAccountByIdValidator();
                var results = validator.Validate(id);
                if (!results.IsValid)
                    return Result<AccountResponse>.Failure(results.Errors.Select(s => s.ErrorMessage));

                var result = await _accountRepository.GetById(id);
                return Result<AccountResponse>.Success(MapResponse(result));
            }
            catch (Exception ex)
            {
                return Result<AccountResponse>.Failure(new List<string> { ex.Message });
            }
        }

        public async Task<Result<AccountResponse>> Update(Guid id, UpdateAccountRequest request)
        {
            try
            {
                var account = await _accountRepository.GetById(id);
                var validator = new UpdateAccountValidator(id, account != null);
                var results = validator.Validate(request);
                if (!results.IsValid)
                    return Result<AccountResponse>.Failure(results.Errors.Select(s => s.ErrorMessage));

                _unitOfWork.BeginTransaction();
                var updatedAccount = await _accountRepository.Update(new Account
                {
                    Id = request.Id,
                    Name = request.Name,
                    Balance = request.Balance
                });

                var updatedAccRecord = await _accountRecordRepository.Create(new AccountRecord
                {
                    AccountId = updatedAccount.Id,
                    Date = DateTime.Now,
                    Value = account.Balance - request.Balance,
                    //Type = (account.Balance - request.Balance) > account.Balance ? 
                });
                _unitOfWork.Commit();

                return Result<AccountResponse>.Success(MapResponse(updatedAccount));
            }
            catch (Exception ex)
            {
                return Result<AccountResponse>.Failure(new List<string> { ex.Message });
            }
        }

        private static AccountResponse MapResponse(Account account)
        {
            return new AccountResponse
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance,
            };
        }

        private static List<AccountResponse> MapResponseList(List<Account> accounts)
        {
            var response = new List<AccountResponse>();

            foreach (var acc in accounts)
            {
                response.Add(new AccountResponse
                {
                    Id = acc.Id,
                    Name = acc.Name,
                    Balance = acc.Balance,
                });
            }

            return response;
        }
    }
}
