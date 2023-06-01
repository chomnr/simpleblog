using Application;
using Application.Common;
using Application.Common.Interface;
using Application.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Account.Pages.Register;

public class RegisterConfirmationModel : PageModel
{
    public string? Email { get; set; }
    
    public bool Token { get; set; }
    
    public virtual Task<IActionResult> OnGetAsync(string email, string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class RegisterConfirmationModel<TUser> : RegisterConfirmationModel where TUser : class
{
    private readonly UserManager<BlogUser> _userManager;
    private readonly IWebHelperService _webHelper;

    public RegisterConfirmationModel(UserManager<BlogUser> userManager, IWebHelperService webHelper)
    {
        _userManager = userManager;
        _webHelper = webHelper;
    }

    public override async Task<IActionResult> OnGetAsync(string? email , string? returnUrl = null)
    {
        /*
        if (email == null)
        {
            Response.Redirect(_webHelper.GetPathBase());
        }
        if ( email != null && !Constraints.IsValidEmail(email))
        {
            Response.Redirect(_webHelper.GetPathBase());
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            Response.Redirect(_webHelper.GetPathBase());
        }
        else
        {
            if (user.EmailConfirmed == true)
            {
                Response.Redirect(_webHelper.GetPathBase());
            }
        }
        */
        
        returnUrl = returnUrl ?? Url.Content("~/");
        
        return Page();
    }
}