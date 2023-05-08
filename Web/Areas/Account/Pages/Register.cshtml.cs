using System.Diagnostics.CodeAnalysis;
using Application.Entities;
using Application.Features.BlogUser;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Web.Areas.Account.Pages;

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
    private readonly UserManager<BlogUser> _userManager;

    public MediatorRegisterModel(IMediator mediator, UserManager<BlogUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }
    
    public override async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync();
        
        ReturnUrl = returnUrl;
    }
    //redirect
    //Response.Redirect("/account/registration/successful");
    // make a create account page that renders after register successful.
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
                return RedirectToPage("register/confirmation", new { email = Input.Email, returnUrl = returnUrl });
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
                }
                return Page();
            }
        }
        return Page();
    }
}
