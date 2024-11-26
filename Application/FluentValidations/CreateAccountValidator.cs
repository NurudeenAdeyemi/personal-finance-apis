using FluentValidation;
using static Application.Commands.CreateAccount;

namespace Application.FluentValidations
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ICNumber).NotEmpty();
            RuleFor(x => x.MobileNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.AcceptTerms).Equal(true).WithMessage("You must accept the terms.");
            RuleFor(x => x.Pin)
            .NotEmpty()
            .WithMessage("Pin is required.")
            .Length(6)
            .WithMessage("Pin must be exactly 6 digits.");

            RuleFor(x => x.ConfirmPin)
                .NotEmpty()
                .WithMessage("Confirm Pin is required.")
                .Equal(x => x.Pin)
                .WithMessage("Confirm Pin must match Pin.");
        }
    }

}
