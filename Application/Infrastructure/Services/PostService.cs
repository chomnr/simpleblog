using Application.Common.Interface;
using Application.Features.Post;
using Application.Infrastructure.Persistence;

namespace Application.Infrastructure.Services;

public class PostService : IPostService
{
    private readonly DatabaseDbContext _context;

    public PostService(DatabaseDbContext context)
    {
        _context = context;
    }

    public async Task<string> CustomCreateAsync(CreatePostCommand command)
    {
        // length and stuff o....
        return "";
    }
}