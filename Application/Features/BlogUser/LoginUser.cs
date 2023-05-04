using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.BlogUser;

public class LoginUser : FeatureController
{
    private readonly IMediator _mediator;

    public LoginUser(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<bool>> Login(LoginCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class LoginCommand : IRequest<bool>
{   
    [Required]
    public string Login { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    public bool RememberMe { get; set; }
}

internal sealed class LoginAccountCommandHandler : IRequestHandler<LoginCommand, bool>
{
    private readonly SignInManager<Entities.BlogUser> _signInManager;
    private readonly UserManager<Entities.BlogUser> _userManager;
    
    public LoginAccountCommandHandler(SignInManager<Entities.BlogUser> signManager,
        UserManager<Entities.BlogUser> userManager)
    {
        _signInManager = signManager;
        _userManager = userManager;
    }
    
    public async Task<bool> Handle(LoginCommand payLoad, CancellationToken cancellationToken)
    {
        if (Utilities.isValidEmail(payLoad.Login, false))
        {
            var user = await _userManager.FindByEmailAsync(payLoad.Login);
            if (user != null)
            {
                var payloadResult = await _signInManager.PasswordSignInAsync(user.UserName,
                    payLoad.Password,
                    payLoad.RememberMe,
                    false);
                Console.WriteLine(user.UserName);
                return payloadResult.Succeeded;
            }
            else
            {
                return false;
            }
        }
        // Login by Username
        var payLoadResult =  await _signInManager.PasswordSignInAsync(payLoad.Login, 
            payLoad.Password, 
            payLoad.RememberMe,
            false);
        return payLoadResult.Succeeded;
    }
    
    public class LoginUserEvent : DomainEvent
    {
        public LoginUserEvent(Entities.BlogUser account)
        {
            Account = account;
        }

        public Entities.BlogUser Account { get; }
    }
}
