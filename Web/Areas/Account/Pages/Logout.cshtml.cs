using System.Diagnostics.CodeAnalysis;
using Application.Common.Interface;
using Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Account.Pages;

public class LogoutModel : PageModel
{
    public virtual Task OnGetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult>
        OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) =>
        throw new NotImplementedException();
}

internal sealed class LogoutModel<TUser> : MediatorLoginModel where TUser : class
{
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly IWebHelperService _webHelper;

    public LogoutModel(SignInManager<BlogUser> signInManager, IWebHelperService webHelper)
    {
        _signInManager = signInManager;
        _webHelper = webHelper;
    }
    
    public override async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }
        
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            Redirect("/");
            await _signInManager.SignOutAsync();
        }
    }
}