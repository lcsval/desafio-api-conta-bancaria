using Desafio.Application.Validators;
using Desafio.Domain;
using Desafio.Domain.Entities;
using Desafio.Domain.Enums;
using Desafio.Domain.Interfaces;
using Desafio.Domain.Interfaces.Repositories;
using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Requests;
using Desafio.Domain.Requests.Customer;
using Desafio.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Application.Services
{
    public class BankService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRecordRepository _accountRecordRepository;

        public BankService
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
                    Operation = AccountOperationEnum.Nenhum,
                    Value = request.Balance,
                    Type = AccountTypeEnum.Credito,
                    Tax = 0,
                    TotalValue = request.Balance
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

        public async Task<Result<ExtractResponse>> Extract(Guid accountId)
        {
            try
            {
                var account = await _accountRepository.GetById(accountId);
                var validator = new ExtractValidator(account != null);
                var results = validator.Validate(accountId);
                if (!results.IsValid)
                    return Result<ExtractResponse>.Failure(results.Errors.Select(s => s.ErrorMessage));

                return Result<ExtractResponse>.Success(await MapExtractResultsResponse(accountId, account));
            }
            catch (Exception ex)
            {
                return Result<ExtractResponse>.Failure(new List<string> { ex.Message });
            }
        }

        public async Task<Result<DepositResponse>> Deposit(DepositRequest request)
        {
            try
            {
                var account = await _accountRepository.GetById(request.AccountId);
                var validator = new DepositValidator(account != null);
                var results = validator.Validate(request);
                if (!results.IsValid)
                    return Result<DepositResponse>.Failure(results.Errors.Select(s => s.ErrorMessage));

                decimal tax = (request.Value * 1) / 100;
                decimal newBalance = account.Balance + (request.Value - tax);

                _unitOfWork.BeginTransaction();
                var updatedAccount = await _accountRepository.Update(new Account
                {
                    Id = account.Id,
                    Name = account.Name,
                    Balance = newBalance
                });

                var insertedAccRecord = await _accountRecordRepository.Create(new AccountRecord
                {
                    AccountId = updatedAccount.Id,
                    Operation = AccountOperationEnum.Deposito,
                    Value = request.Value,
                    Type = AccountTypeEnum.Credito,
                    Tax = tax,
                    TotalValue = (request.Value - tax)
                });
                _unitOfWork.Commit();

                return Result<DepositResponse>.Success(new DepositResponse
                {
                    AccountId = updatedAccount.Id,
                    Name = updatedAccount.Name,
                    NewBalance = updatedAccount.Balance
                });
            }
            catch (Exception ex)
            {
                return Result<DepositResponse>.Failure(new List<string> { ex.Message });
            }
        }

        public async Task<Result<WithdrawResponse>> Withdraw(WithdrawRequest request)
        {
            try
            {
                var account = await _accountRepository.GetById(request.AccountId);
                var validator = new WithdrawValidator(account != null, account?.Balance);
                var results = validator.Validate(request);
                if (!results.IsValid)
                    return Result<WithdrawResponse>.Failure(results.Errors.Select(s => s.ErrorMessage));

                decimal tax = 4;
                decimal newBalance = account.Balance - (request.Value - tax);

                _unitOfWork.BeginTransaction();
                var updatedAccount = await _accountRepository.Update(new Account
                {
                    Id = account.Id,
                    Name = account.Name,
                    Balance = newBalance
                });

                var insertedAccRecord = await _accountRecordRepository.Create(new AccountRecord
                {
                    AccountId = updatedAccount.Id,
                    Operation = AccountOperationEnum.Saque,
                    Value = request.Value,
                    Type = AccountTypeEnum.Debito,
                    Tax = tax,
                    TotalValue = (request.Value - tax)
                });
                _unitOfWork.Commit();

                return Result<WithdrawResponse>.Success(new WithdrawResponse
                {
                    AccountId = updatedAccount.Id,
                    Name = updatedAccount.Name,
                    NewBalance = updatedAccount.Balance
                });
            }
            catch (Exception ex)
            {
                return Result<WithdrawResponse>.Failure(new List<string> { ex.Message });
            }
        }

        public async Task<Result<TransferResponse>> Transfer(TransferRequest request)
        {
            try
            {
                var originAccount = await _accountRepository.GetById(request.OriginAccountId);
                var destinationAccount = await _accountRepository.GetById(request.DestinationAccountId);

                var validator = new TransferValidator(originAccount != null, destinationAccount != null, originAccount?.Balance);
                var results = validator.Validate(request);
                if (!results.IsValid)
                    return Result<TransferResponse>.Failure(results.Errors.Select(s => s.ErrorMessage));

                decimal tax = 1;
                decimal originAccountNewBalance = originAccount.Balance - (request.Value - tax);
                decimal destinationAccountNewBalance = destinationAccount.Balance + (request.Value - tax);

                _unitOfWork.BeginTransaction();
                await RegisterTransfer(request.Value, originAccount, tax, originAccountNewBalance, AccountTypeEnum.Debito);
                await RegisterTransfer(request.Value, destinationAccount, tax, destinationAccountNewBalance, AccountTypeEnum.Credito);
                _unitOfWork.Commit();

                return Result<TransferResponse>.Success(new TransferResponse
                {
                    OriginAccountId = originAccount.Id,
                    OriginAccountName = originAccount.Name,
                    OriginAccountNewBalance = originAccountNewBalance,
                    DestinationAccountId = destinationAccount.Id,
                    DestinationAccountName = destinationAccount.Name,
                    DestinationAccountNewBalance = destinationAccountNewBalance,
                });
            }
            catch (Exception ex)
            {
                return Result<TransferResponse>.Failure(new List<string> { ex.Message });
            }
        }

        private async Task RegisterTransfer(decimal value, Account account, decimal tax, decimal newBalance, string type)
        {
            var updatedAccount = await _accountRepository.Update(new Account
            {
                Id = account.Id,
                Name = account.Name,
                Balance = newBalance
            });

            var insertedAccRecord = await _accountRecordRepository.Create(new AccountRecord
            {
                AccountId = updatedAccount.Id,
                Operation = AccountOperationEnum.Transferencia,
                Value = value,
                Type = type,
                Tax = tax,
                TotalValue = (value - tax)
            });
        }

        #region Maps

        private async Task<ExtractResponse> MapExtractResultsResponse(Guid accountId, Account account)
        {
            var accountRecords = await _accountRecordRepository.GetAll(accountId);

            var rcs = new List<AccountRecordResponse>();
            foreach (var acc in accountRecords)
            {
                rcs.Add(new AccountRecordResponse
                {
                    Date = acc.Date,
                    Operation = acc.Operation,
                    Value = acc.Value,
                    Type = acc.Type,
                    Tax = acc.Tax,
                    TotalValue = acc.TotalValue,
                });
            };

            var result = new ExtractResponse();
            result.AccountId = account.Id;
            result.Name = account.Name;
            result.Balance = account.Balance;
            result.Records = rcs;
            return result;
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

        #endregion Maps
    }
}
