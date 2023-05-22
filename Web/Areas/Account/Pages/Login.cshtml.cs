using System.Diagnostics.CodeAnalysis;
using Application.Entities;
using Application.Features.BlogUser;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Account.Pages;

public abstract class MediatorLoginModel : PageModel
{   
    [BindProperty]
    public LoginCommand Input { get; set; } = default!;
    
    public string? ReturnUrl { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }
    
    public virtual Task OnGetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorLoginModel<TUser> : MediatorLoginModel where TUser : class
{
    private readonly IMediator _mediator;
    private readonly SignInManager<BlogUser> _signInManager;

    public MediatorLoginModel(IMediator mediator, SignInManager<BlogUser> signInManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
    }
    
    // todo fix.
    public override async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            Response.Redirect("/");
        }
        
        ReturnUrl = returnUrl;
    }
    
    public override async Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _mediator.Send(Input);
            if (result.Succeeded)
            {
                Response.Redirect("/");
            }
            else
            {
                ModelState.AddModelError("Input.Login", "Either the username or password is wrong.");
                return Page();
            }
        }
        return Page();
    }
}
