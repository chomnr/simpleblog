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
    private readonly UserManager<Entities.BlogUser> _accountManager;

    public ConfirmAccountQueryHandler(UserManager<Entities.BlogUser> accountManager)
    {
        _accountManager = accountManager;
    }

    public async Task<string> Handle(ConfirmAccountQuery payLoad, CancellationToken cancellationToken)
    {
        var account = await _accountManager.FindByIdAsync(payLoad.UserId);
        if (account != null)
        {
            if (account.EmailConfirmed)
            {
                return "Email is already Confirmed..."; /* Email is already Confirmed redirect or add fancy html &css*/
            }
            
            var payLoadResult = await _accountManager.ConfirmEmailAsync(account, payLoad.VerifyToken ?? "");
            var payLoadSuccess = payLoadResult.Succeeded;
            if (payLoadSuccess)
            {
                account.Done = true;
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