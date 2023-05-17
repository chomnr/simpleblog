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
    
    public IdentityError InvalidName()
    {
        return new IdentityError
        {
            Code = "WrongConstraints",
            Description = "Either the first or last name do not follow the constraints."
        };
    }

    public IdentityError InputLengthTooSmall(string input, int minLength)
    {
        return new IdentityError
        {
            Code = "InputTooSmall",
            Description = input + " is too small the minimum length must be " + minLength
        }; 
    }
    
    public IdentityError InputLengthTooLong(string input, int maxLength)
    {
        return new IdentityError
        {
            Code = "InputTooSmall",
            Description = input + " is too long the maximum length must be " + maxLength
        }; 
    }
}