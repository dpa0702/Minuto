using System;
using FluentValidation;
using FluentValidation.Results;
using Demetrios.Models;

namespace Demetrios.Validation
{
    public class MinutoPostValidator : AbstractValidator<MinutoPost>
    {
        public MinutoPostValidator()
        {
            RuleFor(m => m.Nome).NotNull().WithMessage("Nome que descreva o Minuto.");

            RuleFor(m => m.Canal).NotNull().WithMessage("Tipo de canal de Minuto, podendo ser email, celular ou fixo.");

            RuleFor(m => m.Valor).NotNull().WithMessage("Valor para o canal de Minuto.");

            RuleFor(m => m.Obs).NotNull().WithMessage("Qualquer observação que seja pertinente.");
        }

        protected override bool PreValidate(ValidationContext<MinutoPost> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure("", "Por favor submeta um modelo diferente de nulo."));

                return false;
            }
            return true;
        }
    }
}
