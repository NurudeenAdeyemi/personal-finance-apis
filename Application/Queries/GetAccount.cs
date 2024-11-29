using Application.Exceptions;
using Application.Interfaces.Repositories;
using MediatR;


namespace Application.Queries
{
    public class GetAccount
    {
        public record GetAccountQuery(string ICNumber) : IRequest<GetAccountResponse>;

        public record GetAccountResponse(Guid Id, string ICNumber, string Name, string MobileNumber, string Email, bool MobileNumberConfirmed, bool EmailConfirmed, bool PinSetup, bool BiometricEnabled);

        public class GetAccountHandler : IRequestHandler<GetAccountQuery, GetAccountResponse>
        {
            private readonly IAccountRepository _accountRepository;

            public GetAccountHandler(IAccountRepository accountRepository)
            {
                _accountRepository = accountRepository;
            }

            public async Task<GetAccountResponse> Handle(GetAccountQuery request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindByICNumberAsync(request.ICNumber);

                if (account == null)
                {
                    throw new CustomException($"Account not found.", Exceptions.ExceptionCodes.AccountNotExist.ToString(), 404);
                }

                var response = new GetAccountResponse(account.Id, account.ICNumber, account.Name, account.MobileNumber, account.Email, account.MobileNumberConfirmed, account.EmailConfirmed, account.PinSetup, account.BiometricEnabled);

                return response;
            }
        }
    }
}
