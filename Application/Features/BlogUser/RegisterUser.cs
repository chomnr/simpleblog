using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Features.BlogUser;

public class RegisterUser : FeatureController
{
    private readonly IMediator _mediator;
    
    public RegisterUser(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<IdentityResult>> Register(RegisterCommand command)
    {   
        var sender = await _mediator.Send(command);
        return await _mediator.Send(command);
    }
}

public class RegisterCommand : IRequest<IdentityResult>
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

internal sealed class RegisterAccountCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
{
    private readonly ICustomIdentityService _customIdentityService;
    private readonly IEmailSenderService _emailSender;
    private readonly IConfiguration _configuration;
    private readonly UserManager<Entities.BlogUser> _userManager;
    private readonly IWebHelperService _webHelperService;

    public RegisterAccountCommandHandler(
        ICustomIdentityService customIdentityService, 
        IEmailSenderService emailSender,
        IConfiguration configuration,
        UserManager<Entities.BlogUser> userManager,
        IWebHelperService webHelperService)
    {
        _customIdentityService = customIdentityService;
        _emailSender = emailSender;
        _configuration = configuration;
        _userManager = userManager;
        _webHelperService = webHelperService;
    }

    public async Task<IdentityResult> Handle(RegisterCommand payLoad, CancellationToken cancellationToken)
    {
        var user = new Entities.BlogUser { UserName = payLoad.Username, Email = payLoad.Email };
        var config = _configuration.GetSection("Authentication").GetSection("Email");
        var result = await _customIdentityService.CustomCreateAsync(payLoad, user);
        if (result.Succeeded)
        {
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var basePath = _webHelperService.GetBaseUrl();
            var confirmPath = $"/auth/confirm-email" +
                              $"?userId={Uri.EscapeDataString(user.Id)}" +
                              $"&verifyToken={Uri.EscapeDataString(emailToken)}";
            var formatHtml = $"<a href={basePath + confirmPath}>Confirm Account</a>";
            await _emailSender.SendEmailAsync( payLoad.Email, config["EmailConfirmationSubject"], formatHtml);
            return result;
        }
        return result;
    }
    public class RegisterUserEvent : DomainEvent
    {
        public RegisterUserEvent(Entities.BlogUser account)
        {
            Account = account;
        }

        public Entities.BlogUser Account { get; }
    }
}

/*
        if (!Utilities.isValidEmail(payLoad.Email, true))
        {
            // Probably redundant because of the [EmailAddress] annotation on Email Field.
            // i dont trust the annotation.
            //write exceptionhandler for the errors
            return "The email is invalid."; 
        }

        if (payLoad.Password != payLoad.ConfirmPassword)
        {
            return "The passwords do not match.";
        }
        
        var newUser = new Entities.BlogUser
        {
            UserName = payLoad.Username,
            Email = payLoad.Email
        };
        
        var payLoadResult = await _userManager.CreateAsync(newUser, payLoad.Password);
        if (payLoadResult.Succeeded)
        {
            return "Account successfully created.";
        }
        else
        {
            return payLoadResult.Errors.ToString() ?? "Something went wrong.";
        }
        */