using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Post;

public class RetrievePostsByTag : FeatureController
{
    private readonly IMediator _mediator;

    public RetrievePostsByTag(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<JsonResult>> Login(RetrievePostsByTagCommand command)
    {
        return await _mediator.Send(command);
    }
}

public class RetrievePostsByTagCommand : IRequest<JsonResult>
{
    public int Page { get; set; }
    public string Tag { get; set; }
}

internal sealed class RetrievePostsByTagCommandHandler : IRequestHandler<RetrievePostsByTagCommand, JsonResult>
{
    private readonly IPostService _postService;
    
    public RetrievePostsByTagCommandHandler(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<JsonResult> Handle(RetrievePostsByTagCommand payLoad, CancellationToken cancellationToken)
    {
        return await _postService.RetrieveAllByTag(payLoad);
    }
}