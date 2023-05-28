using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Post;

public class EditPost : FeatureController
{
    private readonly IMediator _mediator;

    public EditPost(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPatch]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<ActionResult<bool>> Login(EditPostCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class EditPostCommand : IRequest<bool>
{   
    [Required]
    public int PostId { get; set; }
    [StringLength(PostConstraints.MaxTitleLength)]
    [MinLength(PostConstraints.MinTitleLength)]
    public string? Title { get; set; }
    [StringLength(PostConstraints.MaxBodyLength)]
    public string? Body { get; set; }
    public string? Tags { get; set; }
}

internal sealed class EditPostCommandHandler : IRequestHandler<EditPostCommand, bool>
{
    private readonly IPostService _postService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public EditPostCommandHandler(IPostService postService, IHttpContextAccessor httpContextAccessor)
    {
        _postService = postService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(EditPostCommand payLoad, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        return await _postService.EditAsync(payLoad, userId);
    }
    
    public class EditPostEvent : DomainEvent
    {
        public EditPostEvent(Entities.Post post)
        {
            Post = post;
        }

        public Entities.Post Post { get; }
    }
}