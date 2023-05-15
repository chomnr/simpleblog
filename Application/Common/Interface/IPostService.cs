using Application.Features.Post;

namespace Application.Common.Interface;

public interface IPostService
{
    Task<bool> CustomCreateAsync(CreatePostCommand command, string userId);
}