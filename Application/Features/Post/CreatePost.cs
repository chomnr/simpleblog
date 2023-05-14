using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<string>> Login(CreatePostCommand command)
    {
        
        return await _mediator.Send(command);
    }
}

public class CreatePostCommand : IRequest<string>
{   
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[A-Za-z0-9\s.,'-]+$")]
    public string Title { get; set; }
    [Required]
    [StringLength(30000)]
    public string Body { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public List<string> Tags { get; set; }
}

internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, string>
{

    private readonly IPostService _postService;
    
    public CreatePostCommandHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<string> Handle(CreatePostCommand payLoad, CancellationToken cancellationToken)
    {
       // _context.Posts
       //_postService.CustomCreateAsync()
       return "";
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