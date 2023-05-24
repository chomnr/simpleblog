using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Post;

public class DeletePost : FeatureController
{
    private readonly IMediator _mediator;

    public DeletePost(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<ActionResult<bool>> Login(DeletePostCommand command)
    {
        
        return await _mediator.Send(command);
    }
}

public class DeletePostCommand : IRequest<bool>
{   
    [Required]
    public int PostId { get; set; }
}

internal sealed class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
{
    private readonly IPostService _postService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DeletePostCommandHandler(IPostService postService, IHttpContextAccessor httpContextAccessor)
    {
        _postService = postService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeletePostCommand payLoad, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        return await _postService.DeleteAsync(payLoad, userId, role);
    }
    
    public class DeletePostEvent : DomainEvent
    {
        public DeletePostEvent(Entities.Post post)
        {
            Post = post;
        }

        public Entities.Post Post { get; }
    }
}