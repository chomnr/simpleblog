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
}