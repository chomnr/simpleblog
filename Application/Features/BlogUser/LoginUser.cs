﻿using System.ComponentModel.DataAnnotations;
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
    public async Task<ActionResult<string>> Login(LoginCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class LoginCommand : IRequest<string>
{   
    [Required]
    public string Login { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

internal sealed class LoginAccountCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly SignInManager<Entities.BlogUser> _signInManager;
    
    public LoginAccountCommandHandler(SignInManager<Entities.BlogUser> signManager)
    {
        _signInManager = signManager;
    }
    
    public async Task<string> Handle(LoginCommand payLoad, CancellationToken cancellationToken)
    {

        //check for whether it is email or username...
        //var payLoadAccount = new Entities.BlogUser { UserName = payLoad.Login };
        var payLoadResult =  await _signInManager.PasswordSignInAsync(payLoad.Login, 
            payLoad.Password, 
            false,
            false);
        return payLoadResult.ToString();
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
