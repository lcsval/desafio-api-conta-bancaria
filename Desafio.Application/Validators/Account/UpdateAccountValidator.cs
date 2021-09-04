using Desafio.Domain.Requests.Customer;
using FluentValidation;
using System;

namespace Desafio.Application.Validators.Account
{
    public class UpdateAccountValidator : AbstractValidator<UpdateAccountRequest>
    {
        public UpdateAccountValidator(Guid idRote, bool validAccount)
        {
            RuleFor(r => r).Custom((entity, context) =>
            {
                if (idRote != entity.Id)
                    context.AddFailure("O Id informado na rota é diferente do Id informado na entidade");

                if (!validAccount)
                    context.AddFailure("A conta informada não existe ou não é válida");

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
