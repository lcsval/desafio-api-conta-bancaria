using Desafio.Domain.Requests;
using FluentValidation;

namespace Desafio.Application.Validators
{
    public class TransferValidator : AbstractValidator<TransferRequest>
    {
        public TransferValidator(bool validOriginAccount, bool validDestinationAccount, decimal? balanceOriginAccount = 0)
        {
            RuleFor(r => r).Custom((entity, context) =>
            {
                if (!validOriginAccount)
                    context.AddFailure("A conta de origem informada não existe ou não é válida");

                if (!validDestinationAccount)
                    context.AddFailure("A conta de destino informada não existe ou não é válida");

                if (entity.Value <= 0)
                    context.AddFailure("[Valor] O valor para transferência deve ser maior que zero");

                if (entity.Value > balanceOriginAccount)
                    context.AddFailure("[Valor] O valor para transferência é maior que o saldo da conta de origem");
            });
        }
    }
}
