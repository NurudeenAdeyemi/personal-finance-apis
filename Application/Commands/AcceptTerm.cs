using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class AcceptTerm
    {
        public record AcceptTermCommand(string ICNumber, bool Term) : IRequest<Result<AccountResponse>>;

        public record AccountResponse(Guid Id, string ICNumber, string Name, string MobileNumber, string Email, bool MobileNumberConfirmed, bool EmailConfirmed, bool PinSetup, bool BiometricEnabled);

        public class Handler : IRequestHandler<AcceptTermCommand, Result<AccountResponse>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<AcceptTermCommand> _logger;

            public Handler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<AcceptTermCommand> logger)
            {
                _accountRepository = accountRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Result<AccountResponse>> Handle(AcceptTermCommand request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (account == null)
                    throw new CustomException("There is no account registered with the IC number", ExceptionCodes.AccountNotExist.ToString(), 404);

                if (!account.MobileNumberConfirmed || !account.EmailConfirmed)
                {
                    throw new CustomException("Mobile number and email must be verified", ExceptionCodes.VerificationNeeded.ToString(), 400);
                }

                if (!request.Term)
                    throw new CustomException("Terms and Condition must be accepted", ExceptionCodes.AccountNotExist.ToString(), 400);

                account.AcceptTerm(request.Term);

                await _accountRepository.UpdateAsync(account);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var response = new AccountResponse(account.Id, account.Name, account.ICNumber, account.MobileNumber, account.Email, account.MobileNumberConfirmed, account.EmailConfirmed, account.PinSetup, account.BiometricEnabled);
                return Result<AccountResponse>.Success(response, $"Customer accepted terms and conditions successfully");
            }
        }
    }
}
