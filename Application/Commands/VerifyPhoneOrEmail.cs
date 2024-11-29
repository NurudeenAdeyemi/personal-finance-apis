using Application.DTOs;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class VerifyPhoneOrEmail
    {
        public record VerifyPhoneOrEmailCommand(string ICNumber, VerificationType Type, string Code) : IRequest<Result<AccountResponse>>;

        public record AccountResponse(Guid Id, string ICNumber, string Name, string MobileNumber, string Email, bool MobileNumberConfirmed, bool EmailConfirmed, bool PinSetup, bool BiometricEnabled);

        public class Handler : IRequestHandler<VerifyPhoneOrEmailCommand, Result<AccountResponse>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMemoryCache _memoryCache;
            private readonly ILogger<VerifyPhoneOrEmailCommand> _logger;

            public Handler(
                IAccountRepository accountRepository,
                IUnitOfWork unitOfWork,
                IMemoryCache memoryCache,
                ILogger<VerifyPhoneOrEmailCommand> logger)
            {
                _accountRepository = accountRepository;
                _unitOfWork = unitOfWork;
                _memoryCache = memoryCache;
                _logger = logger;
            }

            public async Task<Result<AccountResponse>> Handle(VerifyPhoneOrEmailCommand request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (account == null)
                {
                    _logger.LogInformation("There is no account registered with the IC number: {ICNumber}", request.ICNumber);
                    throw new CustomException("There is no account registered with the IC number", ExceptionCodes.AccountNotExist.ToString(), 404);
                }

                

                var cacheKey = $"{request.ICNumber}_{request.Type}";

                if (!_memoryCache.TryGetValue(cacheKey, out string cachedCode))
                {
                    _logger.LogWarning("Incorrect OTP");
                    throw new CustomException("Incorrect OTP", ExceptionCodes.CodeExpired.ToString(), 400);
                }

                if (cachedCode != request.Code)
                {
                    _logger.LogWarning("Incorrect OTP");
                    throw new CustomException("Incorrect OTP", ExceptionCodes.InvalidCode.ToString(), 400);
                }

                switch (request.Type)
                {
                    case VerificationType.MobileNumber:
                        account.ConfirmMobileNumber();
                        _logger.LogInformation("Mobile number verified successfully for ICNumber {ICNumber}.", request.ICNumber);
                        break;

                    case VerificationType.Email:
                        if (!account.MobileNumberConfirmed)
                        {
                            throw new CustomException("Mobile number must be verified first", ExceptionCodes.VerificationNeeded.ToString(), 400);
                        }
                        account.ConfirmEmail();
                        _logger.LogInformation("Email verified successfully for ICNumber {ICNumber}.", request.ICNumber);
                        break;

                    default:
                        _logger.LogError("Incorrect OTP");
                        throw new CustomException("Incorrect OTP", ExceptionCodes.InvalidVerifyType.ToString(), 400);
                }

                await _accountRepository.UpdateAsync(account);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _memoryCache.Remove(cacheKey);

                var response = new AccountResponse(
                    account.Id,
                    account.ICNumber,
                    account.Name,
                    account.MobileNumber,
                    account.Email,
                    account.MobileNumberConfirmed,
                    account.EmailConfirmed,
                    account.PinSetup,
                    account.BiometricEnabled);

                return Result<AccountResponse>.Success(response, $"Customer {request.Type} verification successful.");
            }
        }
    }
}