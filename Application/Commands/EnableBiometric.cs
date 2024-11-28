using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class EnableBiometric
    {
        public record EnableBiometricCommand(string ICNumber, bool Enable) : IRequest<string>;


        public class Handler : IRequestHandler<EnableBiometricCommand, string>
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

            public async Task<string> Handle(EnableBiometricCommand request, CancellationToken cancellationToken)
            {
                var existingCustomer = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (existingCustomer == null)
                    throw new CustomException("Customer with this IC number does not exists.", ExceptionCodes.AccountNotExist.ToString(), 404);

              
                existingCustomer.UpdateBiometricSetting(request.Enable);

                await _accountRepository.UpdateAsync(existingCustomer);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Customer biometric enabled successfully");

                return "Customer biometric enabled successfully";
            }
        }
    }
}
