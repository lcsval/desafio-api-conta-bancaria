using FluentValidation;
using System;

namespace Desafio.Application.Validators.Account
{
    public class GetAccountByIdValidator : AbstractValidator<Guid>
    {
        public GetAccountByIdValidator()
        {
            RuleFor(r => r).Custom((id, context) =>
            {
                if (id == Guid.Empty)
                    context.AddFailure("[Id] O id é obrigatório");
            });
        }
    }
}
