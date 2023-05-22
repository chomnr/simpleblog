using System.Diagnostics.CodeAnalysis;
using Application.Entities;
using Application.Features.BlogUser.Recovery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Web.Areas.Account.Pages.Recovery;

public class MediatorResetPassword : PageModel
{
    [BindProperty]
    public PasswordResetCommand Input { get; set; } = default!;
    
    public string? ReturnUrl { get; set; }
    
    public string UserId { get; set; }
    
    public string ResetToken { get; set; }

    public bool IsSuccessful { get; set; } = false;
    
    public virtual Task OnGetAsync(string userId, string token, [StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorResetPassword<TUser> : MediatorResetPassword where TUser : class
{
    private readonly IMediator _mediator;
    private readonly UserManager<BlogUser> _userManager;

    public MediatorResetPassword(IMediator mediator, UserManager<BlogUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }
    
    public override async Task OnGetAsync(string userId, string resetToken, string? returnUrl = null)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(resetToken))
        {
            Response.Redirect("/");
            return;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            Console.Write("USER NULL");
            Response.Redirect("/");
            return;
        }
        
        Input = new PasswordResetCommand
        {
            UserId = userId,
            ResetToken = resetToken
        };
        ReturnUrl = returnUrl;
    }

    public override async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            IsSuccessful = false;
            return Page();
        }
        
        if (ModelState.IsValid)
        {
            var result = await _mediator.Send(Input);

            if (result.Succeeded)
            {
                IsSuccessful = true;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code.ToLower().Contains("token"))
                    {
                        ModelState.AddModelError("Input.ResetToken", error.Description);
                    }
                    if (error.Code.ToLower().Contains("Password"))
                    {
                        ModelState.AddModelError("Input.Password", error.Description);
                    }
                }
                IsSuccessful = false;
            }
        }
        return Page();
    }
}