using System.Diagnostics.CodeAnalysis;
using Application.Features.BlogUser.Recovery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;


namespace Web.Areas.Account.Pages.Recovery;

public class MediatorResetPassword : PageModel
{
    [BindProperty]
    public PasswordResetCommand Input { get; set; } = default!;
    
    public string? ReturnUrl { get; set; }
    
    public string UserId { get; set; }
    
    public string Token { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }
    
    public virtual Task OnGetAsync(string userId, string token, [StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorResetPassword<TUser> : MediatorResetPassword where TUser : class
{
    private readonly IMediator _mediator;

    public MediatorResetPassword(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task OnGetAsync(string userId, string token, string? returnUrl = null)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            Response.Redirect("/");
        }
        
        //check if token is valid and check if user is valid. 
   
        UserId = userId;
        Token = token;
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
            // DO MEDIATOR AND RETURN STATUS IF IT'S SUCCESS OR NOT OK...
        }
        return Page();
    }
}