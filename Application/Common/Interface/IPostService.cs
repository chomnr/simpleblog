using Application.Features.Post;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Interface;

public interface IPostService
{
    Task<bool> CreateAsync(CreatePostCommand command, string userId);
    Task<JsonResult> RetrieveAllAsync(RetrievePostsCommand command, bool partial);
    
    Task<JsonResult> RetrieveSpecificAsync(RetrievePostCommand command);
}