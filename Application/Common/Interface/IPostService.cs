using Application.Features.Post;

namespace Application.Common.Interface;

public interface IPostService
{
    Task<string> CustomCreateAsync(CreatePostCommand command);
}