using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Post;

public class RetrievePost : FeatureController
{
    private readonly IMediator _mediator;

    public RetrievePost(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Produces("application/json")]
    public async Task<ActionResult<JsonResult>> Login(RetrievePostCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class RetrievePostCommand : IRequest<JsonResult>
{
    public int Id { get; set; }
}

internal sealed class RetrievePostCommandHandler : IRequestHandler<RetrievePostCommand, JsonResult>
{
    private readonly IPostService _postService;
    
    public RetrievePostCommandHandler(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<JsonResult> Handle(RetrievePostCommand payLoad, CancellationToken cancellationToken)
    {
        return await _postService.RetrieveSpecificAsync(payLoad);
    }
}