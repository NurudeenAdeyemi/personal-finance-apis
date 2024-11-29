using FluentValidation;
using static Application.Commands.VerifyPhoneOrEmail;

namespace Application.FluentValidations
{
    public class VerifyCodeValidator : AbstractValidator<VerifyPhoneOrEmailCommand>
    {
        public VerifyCodeValidator()
        {
            RuleFor(x => x.ICNumber)
                .NotEmpty()
                .WithMessage("ICNumber is required.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid verification type.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required.")
                .Length(4)
                .WithMessage("Code must be exactly 4 digits.")
                .Matches(@"^\d{4}$")
                .WithMessage("Code must contain only numeric digits.");
        }
    }
}
