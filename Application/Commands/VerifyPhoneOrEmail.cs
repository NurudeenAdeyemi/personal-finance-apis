using Application.Exceptions;
using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class VerifyPhoneOrEmail
    {
        public record VerifyPhoneOrEmailCommand(string ICNumber, VerifyType Type, string Code) : IRequest<string>;

        public enum VerifyType
        {
            MobileNumber,
            Email
        }

        public class Handler : IRequestHandler<VerifyPhoneOrEmailCommand, string>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<VerifyPhoneOrEmailCommand> _logger;

            public Handler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<VerifyPhoneOrEmailCommand> logger)
            {
                _accountRepository = accountRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<string> Handle(VerifyPhoneOrEmailCommand request, CancellationToken cancellationToken)
            {
                var existingCustomer = await _accountRepository.FindByICNumberAsync(request.ICNumber);
                if (existingCustomer == null)
                    throw new CustomException("Customer with this IC number does not exists.", ExceptionCodes.AccountNotExist.ToString(), 404);

               //validate code

                if(request.Type == VerifyType.MobileNumber)
                {
                    existingCustomer.ConfirmMobileNumber();
                }
                else if(request.Type == VerifyType.Email)
                {
                    existingCustomer.ConfirmEmail();
                }
                else
                {
                    throw new CustomException("Invalid code verification", ExceptionCodes.InvalidVerifyType.ToString(), 400);
                }            

                await _accountRepository.UpdateAsync(existingCustomer);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Customer verification successful");

                return "Customer verification successful";
            }
        }
    }
}
