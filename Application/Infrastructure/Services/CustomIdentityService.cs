using Application.Common;
using Application.Common.Interface;
using Application.Entities;
using Application.Features.BlogUser;
using Microsoft.AspNetCore.Identity;

namespace Application.Infrastructure.Services;

public class CustomIdentityService : ICustomIdentityService
{
    private readonly UserManager<BlogUser> _userManager;
    
    public CustomIdentityService(UserManager<BlogUser> userManager)
    {
        _userManager = userManager;
    }
    
    //todo: add send emailservice...
    public async Task<IdentityResult> CustomCreateAsync(RegisterCommand command, BlogUser user)
    {
        var error = new CustomError();
        var email = command.Email;

        if (!Constraints.IsValidUsername(command.Username))
        {
            return IdentityResult.Failed(error.InvalidName());
        }

        if (!Constraints.IsValidEmail(command.Email))
        {
            return IdentityResult.Failed(error.InvalidEmail()); 
        }
        
        if (!string.Equals(command.Password, command.ConfirmPassword)) 
        {
            return IdentityResult.Failed(error.PasswordDoesNotMatch());
        }
        return await _userManager.CreateAsync(user, command.Password);
    }
}