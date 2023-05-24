using Application.Entities;
using Application.Features.BlogUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Interface;

public interface ICustomIdentityService
{
    // Login
    // Register
    // Delete
    // Update
    
    Task<IdentityResult> CustomCreateAsync(RegisterCommand command, BlogUser user);

    Task<JsonResult> ViewUser(ViewUserCommand command, IPostService postService);
    //CreateCustomAsync
    //Task<BlogUser?> SignInWithEmailOrUsername();
    //Task<BlogUser> RegisterBlogUser();
    //Task<BlogUser> DeleteBlogUser();

}