using AutoFixture;
using Desafio.Application.Services;
using Desafio.Domain.Entities;
using Desafio.Domain.Interfaces;
using Desafio.Domain.Interfaces.Repositories;
using Desafio.Domain.Requests;
using Desafio.Domain.Requests.Customer;
using Desafio.Domain.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Desafio.Tests.Application.Tests.Services
{
    public class BankServiceTest
    {
        private readonly BankService _bankService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IAccountRecordRepository> _accountRecordRepositoryMock;
        private readonly Fixture _fixture;

        public BankServiceTest()
        {
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountRecordRepositoryMock = new Mock<IAccountRecordRepository>();
            _bankService = new BankService(_unitOfWorkMock.Object, _accountRepositoryMock.Object, _accountRecordRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Create_Account()
        {
            var request = _fixture.Build<CreateAccountRequest>()
               .Create();

            var account = _fixture.Create<Account>();
            var accountRecord = _fixture.Create<AccountRecord>();

            _accountRepositoryMock
                .Setup(x => x.Create(It.IsAny<Account>()))
                .ReturnsAsync(account);

            _accountRecordRepositoryMock
                .Setup(x => x.Create(It.IsAny<AccountRecord>()))
                .ReturnsAsync(accountRecord);

            var result = await _bankService.Create(request);

            Assert.True(result.Succeeded);
            Assert.IsType<AccountResponse>(result.Data);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public async Task Should_Not_Create_Account_Because_Validation_Failure()
        {
            var request = _fixture.Build<CreateAccountRequest>()
                .With(w => w.Name, "ab")
                .With(w => w.Balance, -12)
                .Create();

            var result = await _bankService.Create(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "[Nome] O nome deve conter no mínimo 3 caracteres");
            Assert.True(result.Errors.ToList()[1] == "[Saldo] O saldo inicial deve ser superior a zero");
        }

        [Fact]
        public async Task Should_Not_Create_Account_Because_Exception()
        {
            var request = _fixture.Build<CreateAccountRequest>().Create();

            _accountRepositoryMock
                .Setup(x => x.Create(It.IsAny<Account>()))
                .ThrowsAsync(new Exception());

            var result = await _bankService.Create(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async Task Should_GetAll()
        {
            var accounts = _fixture.CreateMany<Account>().ToList();

            _accountRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(accounts);

            var result = await _bankService.GetAll();

            Assert.True(result.Succeeded);
            Assert.IsType<List<AccountResponse>>(result.Data);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public async Task Should_Not_Get_All_Because_Exception()
        {
            _accountRepositoryMock
                .Setup(x => x.GetAll())
                .ThrowsAsync(new Exception());

            var result = await _bankService.GetAll();

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async Task Should_Get_By_Id()
        {
            var account = _fixture.Create<Account>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            var result = await _bankService.GetById(account.Id);

            Assert.True(result.Succeeded);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public async Task Should_Not_Get_By_Id_Because_Invalid_Id()
        {
            var account = _fixture.Create<Account>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            var result = await _bankService.GetById(It.IsAny<Guid>());

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "[Id] O id é obrigatório");
        }

        [Fact]
        public async Task Should_Not_Get_By_Id_Because_Exception()
        {
            var id = _fixture.Create<Guid>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var result = await _bankService.GetById(id);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async Task Should_Get_Extract()
        {
            var account = _fixture.Create<Account>();
            var accountRecord = _fixture.CreateMany<AccountRecord>().ToList();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            _accountRecordRepositoryMock
                .Setup(x => x.GetAll(account.Id))
                .ReturnsAsync(accountRecord);

            var result = await _bankService.Extract(account.Id);

            Assert.True(result.Succeeded);
            Assert.IsType<ExtractResponse>(result.Data);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public async Task Should_Not_Get_Extract_Because_Validation_Failure()
        {
            var result = await _bankService.Extract(It.IsAny<Guid>());

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "A conta informada não existe ou não é válida");
            Assert.True(result.Errors.ToList()[1] == "[AccountId] O id da conta é obrigatório");
        }

        [Fact]
        public async Task Should_Not_Get_Extract_Because_Exception()
        {
            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var result = await _bankService.Extract(It.IsAny<Guid>());

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async Task Should_Deposit()
        {
            var request = _fixture.Create<DepositRequest>();
            var account = _fixture.Create<Account>();
            var accountRecord = _fixture.Create<AccountRecord>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            _accountRepositoryMock
                .Setup(x => x.Update(It.IsAny<Account>()))
                .ReturnsAsync(account);

            _accountRecordRepositoryMock
                .Setup(x => x.Create(accountRecord))
                .ReturnsAsync(accountRecord);

            var result = await _bankService.Deposit(request);

            Assert.True(result.Succeeded);
            Assert.IsType<DepositResponse>(result.Data);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public async Task Should_Not_Deposit_Because_Account_Is_Invalid()
        {
            var request = _fixture.Create<DepositRequest>();

            var result = await _bankService.Deposit(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "A conta informada não existe ou não é válida");
        }

        [Fact]
        public async Task Should_Not_Deposit_Because_Validation_Failure()
        {
            var request = _fixture.Build<DepositRequest>()
                .With(w => w.AccountId, Guid.Empty)
                .With(w => w.Value, -12)
                .Create();

            var result = await _bankService.Deposit(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "A conta informada não existe ou não é válida");
            Assert.True(result.Errors.ToList()[1] == "[Valor] O valor depositado deve ser maior que zero");
        }

        [Fact]
        public async Task Should_Not_Deposit_Because_Exception()
        {
            var request = _fixture.Create<DepositRequest>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var result = await _bankService.Deposit(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async Task Should_Withdraw()
        {
            var request = _fixture.Build<WithdrawRequest>()
                .With(w => w.Value, 2)
                .Create();

            var account = _fixture.Build<Account>()
                .With(w => w.Balance, 2000)
                .Create();

            var accountRecord = _fixture.Create<AccountRecord>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            _accountRepositoryMock
                .Setup(x => x.Update(It.IsAny<Account>()))
                .ReturnsAsync(account);

            _accountRecordRepositoryMock
                .Setup(x => x.Create(accountRecord))
                .ReturnsAsync(accountRecord);

            var result = await _bankService.Withdraw(request);

            Assert.True(result.Succeeded);
            Assert.IsType<WithdrawResponse>(result.Data);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public async Task Should_Not_Withdraw_Because_Account_Is_Invalid()
        {
            var request = _fixture.Create<WithdrawRequest>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()));

            var result = await _bankService.Withdraw(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "A conta informada não existe ou não é válida");
        }

        [Fact]
        public async Task Should_Not_Withdraw_Because_Validation_Failure()
        {
            var request = _fixture.Build<WithdrawRequest>()
                .With(w => w.AccountId, Guid.Empty)
                .With(w => w.Value, -12)
                .Create();

            var result = await _bankService.Withdraw(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "A conta informada não existe ou não é válida");
            Assert.True(result.Errors.ToList()[1] == "[Valor] O valor sacado deve ser maior que zero");
        }

        [Fact]
        public async Task Should_Not_Withdraw_Because_Exception()
        {
            var request = _fixture.Create<WithdrawRequest>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var result = await _bankService.Withdraw(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async Task Should_Transfer()
        {
            var request = _fixture.Build<TransferRequest>()
                .With(w => w.Value, 20)
                .Create();

            var originAccount = _fixture.Build<Account>()
                .With(w => w.Balance, 2000)
                .Create();

            var destinationAccount = _fixture.Build<Account>()
                .With(w => w.Balance, 1000)
                .Create();

            var accountRecord = _fixture.Create<AccountRecord>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(originAccount);

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(destinationAccount);

            _accountRepositoryMock
                .Setup(x => x.Update(It.IsAny<Account>()))
                .ReturnsAsync(originAccount);

            _accountRepositoryMock
                .Setup(x => x.Update(It.IsAny<Account>()))
                .ReturnsAsync(destinationAccount);

            _accountRecordRepositoryMock
                .Setup(x => x.Create(accountRecord))
                .ReturnsAsync(accountRecord);

            var result = await _bankService.Transfer(request);

            Assert.True(result.Succeeded);
            Assert.IsType<TransferResponse>(result.Data);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public async Task Should_Not_Transfer_Because_Accounts_Is_Invalid()
        {
            var request = _fixture.Create<TransferRequest>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()));

            var result = await _bankService.Transfer(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "A conta de origem informada não existe ou não é válida");
            Assert.True(result.Errors.ToList()[1] == "A conta de destino informada não existe ou não é válida");
        }

        [Fact]
        public async Task Should_Not_Transfer_Because_Validation_Failure()
        {
            var request = _fixture.Build<TransferRequest>()
                .With(w => w.Value, -12)
                .Create();

            var originAccount = _fixture.Build<Account>()
                .With(w => w.Balance, -13)
                .Create();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(originAccount);

            var result = await _bankService.Transfer(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "[Valor] O valor para transferência deve ser maior que zero");
            Assert.True(result.Errors.ToList()[1] == "[Valor] O valor para transferência é maior que o saldo da conta de origem");
        }

        [Fact]
        public async Task Should_Not_Transfer_Because_Exception()
        {
            var request = _fixture.Create<TransferRequest>();

            _accountRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var result = await _bankService.Transfer(request);

            Assert.Null(result.Data);
            Assert.True(result.Errors.Count() > 0);
            Assert.True(result.Errors.ToList()[0] == "Exception of type 'System.Exception' was thrown.");
        }
    }
}
