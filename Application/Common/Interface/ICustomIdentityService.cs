using Application.Entities;
using Application.Features.BlogUser;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interface;

public interface ICustomIdentityService
{
    // Login
    // Register
    // Delete
    // Update
    
    Task<IdentityResult> CustomCreateAsync(RegisterCommand payLoad, BlogUser user);
    //CreateCustomAsync
    //Task<BlogUser?> SignInWithEmailOrUsername();
    //Task<BlogUser> RegisterBlogUser();
    //Task<BlogUser> DeleteBlogUser();
}