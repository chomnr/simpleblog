using Application.Common;
using Application.Common.Interface;
using Application.Entities;
using Application.Features.BlogUser;
using Microsoft.AspNetCore.Identity;

using Util = Application.Utilities;

namespace Application.Infrastructure.Services;

public class CustomIdentityService : ICustomIdentityService
{
    private readonly UserManager<BlogUser> _userManager;
    
    public CustomIdentityService(UserManager<BlogUser> userManager)
    {
        _userManager = userManager;
    }
    
    //todo: add send emailservice...
    public async Task<IdentityResult> CustomCreateAsync(RegisterCommand payLoad)
    {
        var error = new CustomError();
        var email = payLoad.Email ?? "";
        
        if (!Util.IsValidEmail(email, true)) 
        {
            return IdentityResult.Failed(error.InvalidEmail());
        }
        
        if (!string.Equals(payLoad.Password, payLoad.ConfirmPassword)) 
        {
            return IdentityResult.Failed(error.PasswordMismatch());
        }

        var blogUser = new BlogUser
        {
            UserName = payLoad.Username,
            Email = payLoad.Email
        };
        var manager = await _userManager.CreateAsync(blogUser, payLoad.Password);
        return manager;
    }
}