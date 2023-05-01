using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Entities;

namespace Application.Features.BlogUser;

public class LoginUserController : FeatureController
{
    private readonly IMediator _mediator;

    public LoginUserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<ActionResult<string>> LoginUser(LoginAccountCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class LoginAccountCommand : IRequest<string>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}


internal sealed class LoginAccountCommandHandler : IRequest<LoginAccountCommand>
{
    private readonly SignInManager<Entities.BlogUser> _signInManager;
    
    public LoginAccountCommandHandler(SignInManager<Entities.BlogUser> signManager)
    {
        _signInManager = signManager;
    }
    
    public async Task<string> Handle(LoginAccountCommand payLoad, CancellationToken cancellationToken)
    {
        //check for whether it is email or username...
        var payLoadAccount = new Entities.BlogUser { UserName = payLoad.Login };
        var payLoadResult = await _signInManager.PasswordSignInAsync(payLoadAccount, 
            payLoad.Password, 
            payLoad.RememberMe, 
            false);
        
        var payLoadSuccess = payLoadResult.Succeeded;
        if (payLoadSuccess)
        {
            return payLoadResult.ToString();
        } else
        {
            return payLoadResult.ToString();
        }
    }
    
    public class LoginAccountEvent : DomainEvent
    {
        public LoginAccountEvent(Entities.BlogUser account)
        {
            Account = account;
        }

        public Entities.BlogUser Account { get; }
    }
}
