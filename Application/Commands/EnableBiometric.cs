using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class EnableBiometric
    {
        public record EnableBiometricCommand(string ICNumber, bool Enable) : IRequest<Result<AccountResponse>>;

        public record AccountResponse(Guid Id, string ICNumber, string Name, string MobileNumber, string Email, bool MobileNumberConfirmed, bool EmailConfirmed, bool TermAccepted, bool PinSetup, bool BiometricEnabled);

        public class Handler : IRequestHandler<EnableBiometricCommand, Result<AccountResponse>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<EnableBiometricCommand> _logger;

            public Handler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<EnableBiometricCommand> logger)
            {
                _accountRepository = accountRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Result<AccountResponse>> Handle(EnableBiometricCommand request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (account == null)
                    throw new CustomException("There is no account registered with the IC number", ExceptionCodes.AccountNotExist.ToString(), 404);

              
                account.UpdateBiometricSetting(request.Enable);

                await _accountRepository.UpdateAsync(account);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Customer biometric enabled successfully");
                var response = new AccountResponse(account.Id, account.Name, account.ICNumber, account.MobileNumber, account.Email, account.MobileNumberConfirmed, account.EmailConfirmed, account.TermAccepted, account.PinSetup, account.BiometricEnabled);
                return Result<AccountResponse>.Success(response, $"Customer biometric enabled successfully");
            }
        }
    }
}
