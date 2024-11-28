using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class CreatePin
    {
        public record CreatePinCommand(string ICNumber, string Pin, string ConfirmPin) : IRequest<string>;


        public class Handler : IRequestHandler<CreatePinCommand, string>
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

            public async Task<string> Handle(CreatePinCommand request, CancellationToken cancellationToken)
            {
                var existingCustomer = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (existingCustomer == null)
                    throw new CustomException("Customer with this IC number does not exists.", ExceptionCodes.AccountNotExist.ToString(), 404);

                var pinHash = BCrypt.Net.BCrypt.HashPassword(request.Pin);

                existingCustomer.SetPin(pinHash);

                await _accountRepository.UpdateAsync(existingCustomer);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Customer account pin created successfully");

                return "Customer account pin created successfully";
            }
        }
    }
}
