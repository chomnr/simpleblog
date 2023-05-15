using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.Common;
using Application.Common.Interface;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Services;
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
    [StringLength(50)]
    [MinLength(10)]
    [RegularExpression(@"^[A-Za-z0-9\s.,'-]+$")]
    public string Title { get; set; }
    [Required]
    [StringLength(30000)]
    public string Body { get; set; }
    [Required]
    public List<string> Tags { get; set; }
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
        return await _postService.CustomCreateAsync(payLoad, userId);
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