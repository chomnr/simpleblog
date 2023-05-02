using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common;


[ApiController]
[Route("api/[controller]")]
public abstract class FeatureController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService(typeof(ISender)) as ISender ?? 
                                                throw new InvalidOperationException("Mediator not found in RequestServices.");
    
}