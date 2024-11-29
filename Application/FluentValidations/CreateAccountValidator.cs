using Application.Commands;
using Application.DTOs;
using FluentValidation;
using MediatR;
using static Application.Commands.CreateAccount;
using static Application.Commands.CreatePin;

namespace Application.FluentValidations
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(x => x.ICNumber)
                .NotEmpty()
                .WithMessage("ICNumber is required.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .WithMessage("Mobile number is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
        }
    }
}
