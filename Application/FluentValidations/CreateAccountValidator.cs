using FluentValidation;
using static Application.Commands.CreateAccount;

namespace Application.FluentValidations
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ICNumber).NotEmpty().Length(10, 20);
            RuleFor(x => x.MobileNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.AcceptTerms).Equal(true).WithMessage("You must accept the terms.");
            RuleFor(x => x.Pin).NotEmpty().Length(6);
        }
    }

}
