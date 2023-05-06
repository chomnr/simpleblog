using Application.Features.BlogUser;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interface;

public interface ICustomIdentityService
{
    // Login
    // Register
    // Delete
    // Update
    
    Task<IdentityResult> CustomCreateAsync(RegisterCommand payLoad);
    //CreateCustomAsync
    //Task<BlogUser?> SignInWithEmailOrUsername();
    //Task<BlogUser> RegisterBlogUser();
    //Task<BlogUser> DeleteBlogUser();
}