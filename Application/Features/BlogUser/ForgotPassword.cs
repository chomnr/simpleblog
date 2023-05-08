using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.BlogUser;

public class ForgotPassword : FeatureController
{
    [HttpGet("/account/forgotpassword")]
    public async Task<ActionResult<IdentityResult>> ResetPassword(string userId, string resetToken)
    {
        var accountConfirm = new ForgotPasswordCommand { UserId = userId, ResetToken = resetToken };
        return await Mediator.Send(accountConfirm);
    }
}

public class ForgotPasswordCommand : IRequest<IdentityResult>
{
    public string? UserId { get; init; }
    public string? ResetToken { get; init; }
    [Required]
    public string? NewPassword { get; set; }
    [Required]
    public string? ConfirmNewPassword { get; set; }
}

public class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordCommand, IdentityResult>
{
    private readonly UserManager<Entities.BlogUser> _userManager;
    private readonly ICustomIdentityService _customIdentityService;

    public ForgotPasswordQueryHandler(UserManager<Entities.BlogUser> userManager,
        ICustomIdentityService customIdentityService)
    {
        _userManager = userManager;
        _customIdentityService = customIdentityService;
    }

    public async Task<IdentityResult> Handle(ForgotPasswordCommand payLoad, CancellationToken cancellationToken)
    {
        return await _customIdentityService.CustomResetPassword(payLoad);
    }
}