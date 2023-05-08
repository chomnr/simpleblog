using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.BlogUser;

public class ConfirmAccount : FeatureController
{
    [HttpGet("/auth/confirm-email")]
    public async Task<ActionResult<string>> GetConfirmEmail(string userId, string verifyToken)
    {
        var accountConfirm = new ConfirmAccountQuery { UserId = userId, VerifyToken = verifyToken };
        return await Mediator.Send(accountConfirm);
    }
}

public class ConfirmAccountQuery : IRequest<string>
{
    public string? UserId { get; init; }
    public string? VerifyToken { get; init; }
}

public class ConfirmAccountQueryHandler : IRequestHandler<ConfirmAccountQuery, string>
{
    private readonly UserManager<Entities.BlogUser> _userManager;

    public ConfirmAccountQueryHandler(UserManager<Entities.BlogUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(ConfirmAccountQuery payLoad, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByIdAsync(payLoad.UserId);
        //todo rewrite emailconfirmation... 
        if (account != null)
        {
            if (account.EmailConfirmed)
            {
                return "Email is already Confirmed..."; /* Email is already Confirmed redirect or add fancy html &css*/
            }

            var payLoadResult = await _userManager.ConfirmEmailAsync(account, payLoad.VerifyToken ?? "");
            var payLoadSuccess = payLoadResult.Succeeded;
            if (payLoadSuccess)
            {
                return "Verified Account"; // some fancy html & css load here.
            }
            else
            {
                return "Failed to Verify account"; // some fancy html & css load here.
            }
        }
        else
        {
            return "Invalid Account"; // redirect
        }
    }
}