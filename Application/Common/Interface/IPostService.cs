using Application.Features.Post;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Interface;

public interface IPostService
{
    Task<bool> CreateAsync(CreatePostCommand command, string userId);
    Task<JsonResult> RetrieveAsync(RetrievePostsCommand command, bool partial);
}