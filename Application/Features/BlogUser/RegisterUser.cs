using System.ComponentModel.DataAnnotations;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.BlogUser;

public class RegisterUser
{
    private readonly IMediator _mediator;

    public RegisterUser(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<string>> Register(RegisterCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class RegisterCommand : IRequest<string>
{   
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}

internal sealed class RegisterAccountCommandHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly UserManager<Entities.BlogUser> _userManager;
    
    public RegisterAccountCommandHandler(UserManager<Entities.BlogUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(RegisterCommand payLoad, CancellationToken cancellationToken)
    {
        if (!Utilities.isValidEmail(payLoad.Email, true))
        {
            // Probably redundant because of the [EmailAddress] annotation on Email Field.
            // i dont trust the annotation.
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