using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class CreatePin
    {
        public record CreatePinCommand(string ICNumber, string Pin, string ConfirmPin) : IRequest<Result<AccountResponse>>;

        public record AccountResponse(Guid Id, string ICNumber, string Name, string MobileNumber, string Email, bool MobileNumberConfirmed, bool EmailConfirmed, bool TermAccepted, bool PinSetup, bool BiometricEnabled);

        public class Handler : IRequestHandler<CreatePinCommand, Result<AccountResponse>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<CreatePinCommand> _logger;

            public Handler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<CreatePinCommand> logger)
            {
                _accountRepository = accountRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Result<AccountResponse>> Handle(CreatePinCommand request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (account == null)
                    throw new CustomException("There is no account registered with the IC number", ExceptionCodes.AccountNotExist.ToString(), 404);

                if(!account.TermAccepted)
                {
                    throw new CustomException("Terms and condition must be accepted", ExceptionCodes.VerificationNeeded.ToString(), 400);
                }

                var pinHash = BCrypt.Net.BCrypt.HashPassword(request.Pin);

                account.SetPin(pinHash);

                await _accountRepository.UpdateAsync(account);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Customer account pin created successfully");
                var response = new AccountResponse(account.Id, account.Name, account.ICNumber, account.MobileNumber, account.Email, account.MobileNumberConfirmed, account.EmailConfirmed, account.TermAccepted, account.PinSetup, account.BiometricEnabled);

                return Result<AccountResponse>.Success(response, $"Customer pin created successfully");
            }
        }
    }
}
