using Desafio.Domain.Requests;
using FluentValidation;

namespace Desafio.Application.Validators
{
    public class WithdrawValidator : AbstractValidator<WithdrawRequest>
    {
        public WithdrawValidator(bool validAccount, decimal? balance = 0)
        {
            RuleFor(r => r).Custom((entity, context) =>
            {
                if (!validAccount)
                    context.AddFailure("A conta informada não existe ou não é válida");

                if (entity.Value <= 0)
                    context.AddFailure("[Valor] O valor sacado deve ser maior que zero");

                if (entity.Value > balance)
                    context.AddFailure("[Valor] O valor sacado é maior que o saldo");
            });
        }
    }
}
