using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Post;

public class CreatePost : FeatureController
{
    private readonly IMediator _mediator;

    public CreatePost(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<ActionResult<bool>> Login(CreatePostCommand command)
    {
        
        return await _mediator.Send(command);
    }
}

public class CreatePostCommand : IRequest<bool>
{   
    [Required]
    [StringLength(PostConstraints.MaxTitleLength)]
    [MinLength(PostConstraints.MinTitleLength)]
    public string Title { get; set; }
    [Required]
    [StringLength(PostConstraints.MaxBodyLength)]
    public string Body { get; set; }
    [Required] 
    [MinLength(PostConstraints.MinTagLength)]
    [MaxLength(PostConstraints.MaxTagLength)]
    public List<string> Tags { get; set; } = new List<string>();
}

internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, bool>
{

    private readonly IPostService _postService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CreatePostCommandHandler(IPostService postService, IHttpContextAccessor httpContextAccessor)
    {
        _postService = postService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(CreatePostCommand payLoad, CancellationToken cancellationToken)
    { 
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _postService.CreateAsync(payLoad, userId);
    }
    
    public class CreatePostEvent : DomainEvent
    {
        public CreatePostEvent(Entities.Post post)
        {
            Post = post;
        }

        public Entities.Post Post { get; }
    }
}