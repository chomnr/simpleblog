using Application.Common;
using Application.Common.Interface;
using Application.Entities;
using Application.Features.BlogUser;
using Microsoft.AspNetCore.Identity;

namespace Application.Infrastructure.Services;

public class CustomSignInService : ICustomSignInService
{
    private readonly UserManager<BlogUser> _userManager;
    private readonly SignInManager<BlogUser> _signInManager;
    
    public CustomSignInService(UserManager<BlogUser> userManager, SignInManager<BlogUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<SignInResult> LoginWithEmailOrUsername(LoginCommand command)
    {
        var login = command.Login;
        var password = command.Password;
        var rememberMe = command.RememberMe;
        
        if (Constraints.IsValidEmail(login))
        {
            var user = await _userManager.FindByEmailAsync(login);
            if (user?.UserName != null)
            {
                return await _signInManager.PasswordSignInAsync(user.UserName, password, rememberMe, false);
            }
            else
            {
                return SignInResult.Failed;
            }
        }
        else
        {
            return await _signInManager.PasswordSignInAsync(login, password, rememberMe, false);
        }
    }
}