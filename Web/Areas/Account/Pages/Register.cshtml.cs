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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Web.Areas.Account.Pages;

[NotAuthorize]
public abstract class MediatorRegisterModel : PageModel
{   
    [BindProperty]
    public RegisterCommand Input { get; set; } = default!;
    
    public string? ReturnUrl { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }

    public virtual Task OnGetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorRegisterModel<TUser> : MediatorRegisterModel where TUser : class
{
    private readonly IMediator _mediator;
    private readonly IWebHelperService _webHelper;

    public MediatorRegisterModel(IMediator mediator, 
        IWebHelperService webHelper,
        SignInManager<BlogUser> signInManager)
    {
        _mediator = mediator;
        _webHelper = webHelper;
    }
    
    public override async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }
        returnUrl ??= Url.Content("~/");
        ReturnUrl = returnUrl;
    }

    
    public override async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        if (ModelState.IsValid)
        {
            var result = await _mediator.Send(Input);
            if (result.Succeeded)
            {
                Response.Redirect(_webHelper.GetPathBase() + "account/register/confirmation");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code.ToLower().Contains("username"))
                    {
                        ModelState.AddModelError("Input.Username", error.Description);
                    }
                    if (error.Code.ToLower().Contains("email"))
                    {
                        ModelState.AddModelError("Input.Email", error.Description);
                    }
                    if (error.Code.ToLower().Contains("password"))
                    {
                        ModelState.AddModelError("Input.Password", error.Description);
                    }
                    if (error.Code.ToLower().Contains("firstname"))
                    {
                        ModelState.AddModelError("Input.FirstName", error.Description);
                    }
                    if (error.Code.ToLower().Contains("lastname"))
                    {
                        ModelState.AddModelError("Input.FirstName", error.Description);
                    }
                }
                return Page();
            }
        }
        return Page();
    }
}
