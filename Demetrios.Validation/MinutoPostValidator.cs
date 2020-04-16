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
            RuleFor(m => m.link).NotNull().WithMessage("Nome que descreva o Minuto.");

            RuleFor(m => m.description).NotNull().WithMessage("Qualquer observação que seja pertinente.");
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
