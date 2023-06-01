using System.Diagnostics.CodeAnalysis;
using Application.Common.Interface;
using Application.Common.Security;
using Application.Entities;
using Application.Features.BlogUser;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SendGrid;

namespace Web.Areas.Account.Pages;

[NotAuthorize]
public abstract class MediatorRecoveryModel : PageModel
{   
    [BindProperty]
    public RecoveryCommand Input { get; set; } = default!;
    
    public string? ReturnUrl { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }
    
    public virtual Task OnGetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorRecoveryModel<TUser> : MediatorRecoveryModel where TUser : class
{
    private readonly IMediator _mediator;
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly IWebHelperService _webHelper;

    public MediatorRecoveryModel(IMediator mediator, SignInManager<BlogUser> signInManager, IWebHelperService webHelper)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _webHelper = webHelper;
    }
    
    // todo fix.
    public override async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }
        returnUrl ??= Url.Content("~/");
        ReturnUrl = returnUrl;
    }
    
    public override async Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            await _mediator.Send(Input);
            Response.Redirect(_webHelper.GetPathBase() + "account/recovery/confirmation");
        }
        return Page();
    }
}