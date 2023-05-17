using Application.Features.Post;

namespace Application.Common.Interface;

public interface IPostService
{
    Task<bool> CreateAsync(CreatePostCommand command, string userId);
}