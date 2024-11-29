using Application.DTOs;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;


namespace Application.Queries
{
    public class GetAccount
    {
        public record GetAccountQuery(string ICNumber) : IRequest<Result<AccountResponse>>;

        public record AccountResponse(Guid Id, string ICNumber, string Name, string MobileNumber, string Email, bool MobileNumberConfirmed, bool EmailConfirmed, bool TermAccepted, bool PinSetup, bool BiometricEnabled);

        public class GetAccountHandler : IRequestHandler<GetAccountQuery, Result<AccountResponse>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IMemoryCache _memoryCache;
            public GetAccountHandler(IAccountRepository accountRepository, IMemoryCache memoryCache)
            {
                _accountRepository = accountRepository;
                _memoryCache = memoryCache;
            }

            public async Task<Result<AccountResponse>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindByICNumberAsync(request.ICNumber);

                if (account == null)
                {
                    throw new CustomException($"There is no account registered with the IC number", ExceptionCodes.AccountNotExist.ToString(), 404);
                }

                var mobileVerificationCode = GenerateVerificationCode();
                var emailVerificationCode = GenerateVerificationCode();

                _memoryCache.Set(GetMobileCacheKey(request.ICNumber), mobileVerificationCode, TimeSpan.FromMinutes(10));
                _memoryCache.Set(GetEmailCacheKey(request.ICNumber), emailVerificationCode, TimeSpan.FromMinutes(10));

                var response = new AccountResponse(account.Id, account.ICNumber, account.Name, account.MobileNumber, account.Email, account.MobileNumberConfirmed, account.EmailConfirmed, account.TermAccepted, account.PinSetup, account.BiometricEnabled);

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
