using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Vendor.Domain.Types;

namespace Vendor.Services.User.CQRS.Commands.User;

public class SendConfirmationEmailCommand : IRequest<ApiResponse>
{
    public string ConfirmationLink { get; set; }
    public string Email { get; set; }

    public SendConfirmationEmailCommand()
    {
    }

    public SendConfirmationEmailCommand(string confirmationLink, string email)
    {
        ConfirmationLink = confirmationLink;
        Email = email;
    }
}

public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand, ApiResponse>
{
    private readonly IEmailSender _emailSender;

    public SendConfirmationEmailCommandHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task<ApiResponse> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        await _emailSender.SendEmailAsync(request.Email, "Please confirm your account!",
            "Please activate your account by clicking <a href=\"" 
            + request.ConfirmationLink 
            + "\"> here!</a>");

        return new ApiResponse();
    }
}