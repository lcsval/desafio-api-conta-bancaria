using Desafio.Domain.Requests.Customer;
using FluentValidation;

namespace Desafio.Application.Validators.Account
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountValidator()
        {
            RuleFor(r => r).Custom((entity, context) =>
            {
                if (entity.Name == null || entity.Name == string.Empty)
                    context.AddFailure("[Nome] O nome é obrigatório");

                if (entity.Name == null || entity.Name.Length < 3)
                    context.AddFailure("[Nome] O nome deve conter no mínimo 3 caracteres");

                if (entity.Balance <= 0)
                    context.AddFailure("[Saldo] O saldo inicial deve ser superior a zero");
            });
        }
    }
}
