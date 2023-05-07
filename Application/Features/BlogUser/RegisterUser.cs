using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    public RegisterAccountCommandHandler(ICustomIdentityService customIdentityService)
    {
        _customIdentityService = customIdentityService;
    }

    public async Task<IdentityResult> Handle(RegisterCommand payLoad, CancellationToken cancellationToken)
    {
        return await _customIdentityService.CustomCreateAsync(payLoad);
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