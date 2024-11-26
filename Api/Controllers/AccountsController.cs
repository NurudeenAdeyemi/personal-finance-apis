using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<CreateAccount.CreateAccountResponse> Register([FromBody] CreateAccount.CreateAccountCommand command)
        {
            var account = await _mediator.Send(command);
            return account;
        }
    }
}
