using System.ComponentModel.DataAnnotations;
using Application.Common;
using MediatR;
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
    public async Task<ActionResult<string>> Login(CreatePostCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class CreatePostCommand : IRequest<string>
{   
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public List<string> Tags { get; set; }
}