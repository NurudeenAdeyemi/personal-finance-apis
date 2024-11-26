using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class CreateAccount
    {
        public record CreateAccountCommand(string Name, string ICNumber, string MobileNumber, string Email, string Pin, bool AcceptTerms, bool EnableBiometric) : IRequest<CreateAccountResponse>;
        
        public record CreateAccountResponse(Guid Id, string Name, string ICNumber, string MobileNumber, string Email);

        public class Handler : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<CreateAccountCommand> _logger;

            public Handler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<CreateAccountCommand> logger)
            {
                _accountRepository = accountRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
            {
                var existingCustomer = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (existingCustomer != null)
                    throw new InvalidOperationException("Customer with this IC number already exists.");

                var pinHash = BCrypt.Net.BCrypt.HashPassword(request.Pin);

                var account =  new Account(request.Name, request.ICNumber, request.MobileNumber, request.Email, pinHash);

                await _accountRepository.AddAsync(account);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Customer account registered successfully: {ICNumber}", request.ICNumber);

                return new CreateAccountResponse(account.Id, account.Name, account.ICNumber, account.MobileNumber, account.Email);
            }
        }
    }
}
