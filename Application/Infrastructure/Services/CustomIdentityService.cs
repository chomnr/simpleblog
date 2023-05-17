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
        
        var username = command.Username;
        var email = command.Email;
        var firstName = command.FirstName;
        var lastName = command.LastName;

        var userNameConstraint = Constraints.IsValidUsername(username);
        var firstNameConstraint = Constraints.IsValidRealName(firstName);
        var lastNameConstraint = Constraints.IsValidRealName(lastName);

        if (!firstNameConstraint.Succeeded)
        {
            return IdentityResult.Failed(firstNameConstraint.Errors.GetEnumerator().Current);
        }
        
        if (!lastNameConstraint.Succeeded)
        {
            return IdentityResult.Failed(lastNameConstraint.Errors.GetEnumerator().Current);
        }

        if (!userNameConstraint.Succeeded)
        {
            return IdentityResult.Failed(userNameConstraint.Errors.GetEnumerator().Current);
        }

        if (!Constraints.IsValidEmail(email))
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