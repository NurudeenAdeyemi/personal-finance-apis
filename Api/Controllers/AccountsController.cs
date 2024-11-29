using Application.Commands;
using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.Queries.GetAccount;
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
        public async Task<IActionResult> Register([FromBody] CreateAccount.CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyPhoneOrEmail.VerifyPhoneOrEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("pin-setup")]
        public async Task<IActionResult> SetupPin([FromBody] CreatePin.CreatePinCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("toggle-biomteric")]
        public async Task<IActionResult> EnableBiometric([FromBody] EnableBiometric.EnableBiometricCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{icNumber}")]
        public async Task<IActionResult> GetAccount(string icNumber)
        {
            var request = await _mediator.Send(new GetAccountQuery(icNumber));
            return Ok(request);
        }
    }
}
