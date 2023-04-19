using Flurl;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Services.User.Api.CQRS.Commands.Token;
using Vendor.Services.User.Api.CQRS.Commands.User;

namespace Vendor.Services.User.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public UserController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Register(CreateUserCommand command)
    {
        var response = await _mediator.Send(command);
        
        if (!response.IsValid)
            return BadRequest(response);
        var generateConfirmationToken = await _mediator.Send(new GenerateConfirmationTokenCommand()
            {Username = response.Result!.Username!});
        
        
        if (!generateConfirmationToken.IsValid)
            return BadRequest(generateConfirmationToken);

        var confirmationLinksBaseAddress = _configuration["EmailConfirmation:BaseAddress"];
        var confirmationLink = confirmationLinksBaseAddress.SetQueryParams(new
            { Token = generateConfirmationToken.Result, Username = response.Result!.Username });

        var sendConfirmationLink =
            await _mediator.Send(new SendConfirmationEmailCommand(confirmationLink!, response.Result!.Email!));
        if (!sendConfirmationLink.IsValid)
            return BadRequest(sendConfirmationLink);
        
        
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(ConfirmUserEmailCommand request)
    {
        var confirmUserEmail = await _mediator.Send(request);
        
        if (!confirmUserEmail.IsValid)
            return BadRequest(confirmUserEmail);
        return Ok(confirmUserEmail);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public async Task<IActionResult> AddRole(AddRoleToUserCommand command)
    {
        var addRoleCommand = await _mediator.Send(command);

        if (!addRoleCommand.IsValid)
            return BadRequest(addRoleCommand);
        return Ok(addRoleCommand);
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