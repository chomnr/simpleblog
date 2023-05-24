using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.BlogUser;

public class ViewUser : FeatureController
{
    private readonly IMediator _mediator;

    public ViewUser(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<JsonResult>> Login(ViewUserCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class ViewUserCommand : IRequest<JsonResult>
{   
    [Required]
    public string Id { get; }
}

internal sealed class ViewUserCommandHandler : IRequestHandler<ViewUserCommand, JsonResult>
{
    private readonly ICustomIdentityService _customIdentity;
    private readonly IPostService _postService;
    
    public ViewUserCommandHandler(ICustomIdentityService customIdentity, IPostService postService)
    {
        _customIdentity = customIdentity;
        _postService = postService;
    }
    
    public async Task<JsonResult> Handle(ViewUserCommand payLoad, CancellationToken cancellationToken)
    {
        return await _customIdentity.ViewUser(payLoad, _postService);
    }
    
    public class ViewUserEvent : DomainEvent
    {
        public ViewUserEvent(Entities.BlogUser account)
        {
            Account = account;
        }

        public Entities.BlogUser Account { get; }
    }
}