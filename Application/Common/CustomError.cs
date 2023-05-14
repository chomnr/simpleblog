using Microsoft.AspNetCore.Identity;

namespace Application.Common;

public class CustomError : IdentityErrorDescriber
{
    public IdentityError InvalidEmail()
    {
        return new IdentityError
        {
            Code = "InvalidEmail",
            Description = "The email does not follow the constraints."
        };
    }
    
    public IdentityError PasswordDoesNotMatch()
    {
        return new IdentityError
        {
            Code = "PasswordDoesNotMatch",
            Description = "The passwords do not match each other."
        };
    }
    
    public IdentityError BadNameConstraints()
    {
        return new IdentityError
        {
            Code = "WrongConstraints",
            Description = "Either the first or last name do not follow the constraints."
        };
    }
}