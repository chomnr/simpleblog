using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Post;

public class RetrievePosts : FeatureController
{
    private readonly IMediator _mediator;

    public RetrievePosts(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Produces("application/json")]
    public async Task<ActionResult<JsonResult>> Login(RetrievePostsCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class RetrievePostsCommand : IRequest<JsonResult>
{
    public int Page { get; set; }
}

internal sealed class RetrievePostsCommandHandler : IRequestHandler<RetrievePostsCommand, JsonResult>
{
    private readonly IPostService _postService;
    
    public RetrievePostsCommandHandler(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<JsonResult> Handle(RetrievePostsCommand payLoad, CancellationToken cancellationToken)
    {
        return await _postService.RetrieveAsync(payLoad, true);
    }
}