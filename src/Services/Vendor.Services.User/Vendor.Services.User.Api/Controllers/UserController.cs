using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Services.User.Commands.User.ConfirmUserEmailCommand;
using Vendor.Services.User.Commands.User.CreateUserCommand;
using Vendor.Services.User.Commands.User.GenerateConfirmationTokenCommand;
using Vendor.Services.User.Commands.User.SendConfirmationEmailCommand;

namespace Vendor.Services.User.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register(CreateUserCommand command)
    {
        var response = await _mediator.Send(command);
        
        if (!response.IsValid)
            return BadRequest(response);
        var generateConfirmationToken = await _mediator.Send(new GenerateConfirmationTokenCommand()
            {Username = response.Result!.UserName!});
        
        
        if (!generateConfirmationToken.IsValid)
            return BadRequest(generateConfirmationToken);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), "User",
            new {Token = generateConfirmationToken.Result, Username = response.Result!.UserName}, Request.Scheme);
        

        var sendConfirmationLink =
            await _mediator.Send(new SendConfirmationEmailCommand(confirmationLink!, response.Result!.Email!));
        if (!sendConfirmationLink.IsValid)
            return BadRequest(sendConfirmationLink);
        
        
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string token, string username)
    {
        var confirmUserEmail = await _mediator.Send(new ConfirmUserEmailCommand(username, token));
        
        if (!confirmUserEmail.IsValid)
            return BadRequest(confirmUserEmail);
        return Ok(confirmUserEmail);
    }
}