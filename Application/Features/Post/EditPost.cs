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
    public string? Title { get; set; }
    public List<String>? Tags { get; set; }
    public string? Body { get; set; }
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
        return await _postService.EditAsync(payLoad);
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