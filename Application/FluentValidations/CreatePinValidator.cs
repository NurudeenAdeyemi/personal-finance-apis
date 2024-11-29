using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Commands.CreatePin;

namespace Application.FluentValidations
{
    public class CreatePinValidator : AbstractValidator<CreatePinCommand>
    {
        public CreatePinValidator()
        {
            RuleFor(x => x.ICNumber).NotEmpty();
            RuleFor(x => x.Pin)
            .NotEmpty()
            .WithMessage("Pin is required.")
            .Length(6)
            .WithMessage("Pin must be exactly 6 digits.")
            .Matches(@"^\d{6}$")
            .WithMessage("Pin must contain only numeric digits.");

            RuleFor(x => x.ConfirmPin)
                .NotEmpty()
                .WithMessage("Confirm Pin is required.")
                .Equal(x => x.Pin)
                .WithMessage("Confirm Pin must match Pin.")
                .Matches(@"^\d{6}$")
                .WithMessage("Pin must contain only numeric digits.");
        }
    }
}
