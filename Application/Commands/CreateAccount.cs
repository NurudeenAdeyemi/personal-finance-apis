using Application.DTOs;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class CreateAccount
    {
        public record CreateAccountCommand(string Name, string ICNumber, string MobileNumber, string Email) : IRequest<Result<AccountResponse>>;

        public record AccountResponse(Guid Id, string ICNumber, string Name, string MobileNumber, string Email, bool MobileNumberConfirmed, bool EmailConfirmed, bool PinSetup, bool BiometricEnabled);

        public class Handler : IRequestHandler<CreateAccountCommand, Result<AccountResponse>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMemoryCache _memoryCache;
            private readonly ILogger<CreateAccountCommand> _logger;

            public Handler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IMemoryCache memoryCache, ILogger<CreateAccountCommand> logger)
            {
                _accountRepository = accountRepository;
                _unitOfWork = unitOfWork;
                _memoryCache = memoryCache;
                _logger = logger;
            }

            public async Task<Result<AccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
            {
                var existingCustomer = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (existingCustomer != null)
                {
                    _logger.LogInformation("Account already exists: {ICNumber}", request.ICNumber);
                    throw new CustomException("Account already exists.", ExceptionCodes.AccountExist.ToString(), 400);
                }

                var account = new Account(request.Name, request.ICNumber, request.MobileNumber, request.Email);
                await _accountRepository.AddAsync(account);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var mobileVerificationCode = GenerateVerificationCode();
                var emailVerificationCode = GenerateVerificationCode();

                _memoryCache.Set(GetMobileCacheKey(request.ICNumber), mobileVerificationCode, TimeSpan.FromMinutes(10));
                _memoryCache.Set(GetEmailCacheKey(request.ICNumber), emailVerificationCode, TimeSpan.FromMinutes(10));

                _logger.LogInformation("Verification codes generated and cached for ICNumber: {ICNumber}", request.ICNumber);

                _logger.LogInformation("Customer account registered successfully: {ICNumber}", request.ICNumber);

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

                return Result<AccountResponse>.Success(
                    response,
                    $"Customer account registered successfully: {request.ICNumber}. Verification codes sent to mobile and email.");
            }

            private static string GenerateVerificationCode()
            {
                var random = new Random();
                return random.Next(1000, 9999).ToString("D4");
            }

            private static string GetMobileCacheKey(string icNumber)
            {
                return $"{icNumber}_{VerificationType.MobileNumber}";
            }

            private static string GetEmailCacheKey(string icNumber)
            {
                return $"{icNumber}_{VerificationType.Email}";
            }
        }
    }
}
