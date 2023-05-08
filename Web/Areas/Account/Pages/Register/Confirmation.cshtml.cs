using Application;
using Application.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Account.Pages.Register;

[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    public string? Email { get; set; }
    
    public bool Token { get; set; }
    
    public virtual Task<IActionResult> OnGetAsync(string email, string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class RegisterConfirmationModel<TUser> : RegisterConfirmationModel where TUser : class
{
    private readonly UserManager<BlogUser> _userManager;

    public RegisterConfirmationModel(UserManager<BlogUser> userManager)
    {
        _userManager = userManager;
    }

    public override async Task<IActionResult> OnGetAsync(string? email , string? returnUrl = null)
    {
        if (email == null)
        {
            Response.Redirect("/");
        }
        if (!Utilities.IsValidEmail(email = "", false))
        {
            Response.Redirect("/");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            Response.Redirect("/");
        }
        else
        {
            if (user.EmailConfirmed == true)
            {
                Response.Redirect("/");
            }
        }
        
        returnUrl = returnUrl ?? Url.Content("~/");
        
        return Page();
    }
}