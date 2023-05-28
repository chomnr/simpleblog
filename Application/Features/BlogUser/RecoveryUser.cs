using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Features.BlogUser;

public class RecoveryUser : FeatureController
{
    private readonly IMediator _mediator;

    public RecoveryUser(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<IdentityResult>> Login(RecoveryCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class RecoveryCommand : IRequest<IdentityResult>
{   
    [Required]
    public string Email { get; set; }
}

internal sealed class RecoveryUserCommandHandler : IRequestHandler<RecoveryCommand, IdentityResult>
{
    private readonly UserManager<Entities.BlogUser> _userManager;
    private readonly IWebHelperService _webHelper;
    private readonly IEmailSenderService _emailSender;
    private readonly IConfiguration _configuration;
    
    public RecoveryUserCommandHandler(UserManager<Entities.BlogUser> userManager, 
        IWebHelperService webHelper,
        IEmailSenderService emailSender,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _webHelper = webHelper;
        _emailSender = emailSender;
        _configuration = configuration;
    }
    
    public async Task<IdentityResult> Handle(RecoveryCommand payLoad, CancellationToken cancellationToken)
    {
        var error = new CustomError();
        var config = _configuration.GetSection("Authentication").GetSection("Email");
        var email = payLoad.Email; 
        
        if (!Constraints.IsValidEmail(email))
        {
            return IdentityResult.Failed(error.InvalidEmail());
        }
    
        var user = await _userManager.FindByEmailAsync(payLoad.Email);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var basePath = _webHelper.GetBaseUrl();
            var confirmPath = $"/account/recovery/resetpassword" +
                              $"?userId={Uri.EscapeDataString(user.Id)}" +
                              $"&resetToken={Uri.EscapeDataString(token)}";
            var passwordConfirmBody = config["EmailResetPasswordBody"]
                .Replace("{url}", basePath + confirmPath)
                .Replace("{token}", token)
                .Replace("{userid}", user.Id)
                .Replace("{firstname}", user.FirstName)
                .Replace("{lastname}", user.LastName);
            
            await _emailSender.SendEmailAsync(payLoad.Email, config["EmailResetPasswordSubject"], passwordConfirmBody);
        }
        // End heres.
        
        return IdentityResult.Failed(error.DefaultError());
    }
    
    public class RecoveryUserEvent : DomainEvent
    {
        public RecoveryUserEvent(Entities.BlogUser user)
        {
            User = user;
        }

        public Entities.BlogUser User { get; }
    }
}