using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace Application.Common;
/*
 
 !!! NOTICE !!!
 
 Username:
 Make sure you adjust the AllowedCharactersRegex accordance to the AllowedCharacters.
 
 Password:
 Make sure you adjust the PasswordRegex accordance to RequireUpperCase, RequireNonAlphanumeric, RequireDigit.
 
 Following these instructions ensure that client side & server side validation are equivalent.
 
 */

public static class Constraints
{
    public static IdentityResult IsValidRealName(string name)
    {
        var error = new CustomError();
        
        var regex = new Regex(UserConstraints.RealNameRegex);

        if (!regex.IsMatch(name)) { return IdentityResult.Failed(error.InvalidName()); }
        if (name.Length > UserConstraints.RealNameMaxLength)
        {
            return IdentityResult.Failed(error.InputLengthTooLong(name, UserConstraints.RealNameMaxLength));
        }
        
        return IdentityResult.Success;
    }
    
    public static IdentityResult IsValidUsername(string username)
    {
        var error = new CustomError();
        
        var regex = new Regex(UsernameConstraints.AllowedCharactersRegex);

        if (!regex.IsMatch(username))
        {
            return IdentityResult.Failed(error.InvalidUserName(username));
        }

        if (username.Length < UsernameConstraints.MinLength)
        {
            return IdentityResult.Failed(error.InputLengthTooSmall(username, UsernameConstraints.MinLength));
        }

        if (username.Length > UsernameConstraints.MaxLength)
        {
            return IdentityResult.Failed(error.InputLengthTooLong(username, UsernameConstraints.MaxLength));
        }
        
        return IdentityResult.Success;
    }

    public static bool IsValidEmail(string email)
    {
        var regex = new Regex(UserConstraints.EmailRegex);
        
        if (!regex.IsMatch(email)) { return false; }
        if (email.Length > UserConstraints.EmailMaxLength) { return false; }
        
        return true;
    }
}
public static class UserConstraints
{
    //public const string RealNameRegex = @"^[\p{L}\p{N}\s[^$&+,:;=?@#|'<>.^*()%!-]+$";
    public const string RealNameRegex = @"^[a-zA-Z]+$";
    public const int RealNameMaxLength = 256;

    public const string EmailRegex = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))){2,63}\.?$";
    public const int EmailMaxLength = 254;
}

public static class UsernameConstraints
{
    public const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
    public const string AllowedCharactersRegex = @"^[a-zA-Z0-9_]+$";
    public const int MinLength = 3;
    public const int MaxLength = 16;
}

public static class PasswordConstraints
{
    public const string PasswordRegex = @"^(?=.*[A-Z])(?=.*\W)(?=.*\d)[A-Za-z\d\W]+$";
    public const bool RequireUpperCase = true;
    public const bool RequireNonAlphanumeric = true;
    public const bool RequireDigit = true;
    public const int MinLength = 7;
    public const int MaxLength = 64;
}

public static class PostConstraints
{
    public const int MinTagLength = 1;
    public const int MaxTagLength = 4;
    public const int MinTagNameLength = 3;
    public const int MaxTagNameLength = 15;
    
    public const int MinTitleLength = 10;
    public const int MaxTitleLength = 50;
    
    public const int MaxBodyLength = 30000;

    public const int RetrievalAmount = 12;
}