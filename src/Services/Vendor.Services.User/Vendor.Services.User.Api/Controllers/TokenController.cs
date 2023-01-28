using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Services.User.Commands.Token.CreateTokenCommand;

namespace Vendor.Services.User.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class TokenController : ControllerBase
{
    private readonly  IMediator _mediator;

    public TokenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login(CreateTokenCommand command)
    {
        var loginResult = await _mediator.Send(command);
        
        if (!loginResult.IsValid)
            return BadRequest(loginResult);
        return Ok(loginResult);
    }
}