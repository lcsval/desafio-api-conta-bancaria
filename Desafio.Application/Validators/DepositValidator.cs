using Desafio.Domain.Requests;
using FluentValidation;

namespace Desafio.Application.Validators
{
    public class DepositValidator : AbstractValidator<DepositRequest>
    {
        public DepositValidator(bool validAccount)
        {
            RuleFor(r => r).Custom((entity, context) =>
            {
                if (!validAccount)
                    context.AddFailure("A conta informada não existe ou não é válida");

                if (entity.Value <= 0)
                    context.AddFailure("[Valor] O valor depositado deve ser maior que zero");
            });
        }
    }
}
