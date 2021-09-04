using FluentValidation;
using System;

namespace Desafio.Application.Validators
{
    public class ExtractValidator : AbstractValidator<Guid>
    {
        public ExtractValidator(bool validAccount)
        {
            RuleFor(r => r).Custom((accountId, context) =>
            {
                if (!validAccount)
                    context.AddFailure("A conta informada não existe ou não é válida");

                if (accountId == Guid.Empty)
                    context.AddFailure("[AccountId] O id da conta é obrigatório");
            });
        }
    }
}
