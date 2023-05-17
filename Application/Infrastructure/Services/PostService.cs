using Application.Common;
using Application.Common.Interface;
using Application.Entities;
using Application.Features.Post;
using Application.Infrastructure.Persistence;

namespace Application.Infrastructure.Services;

public class PostService : IPostService
{
    /* TODO: ADD CUSTOM ERRORS FOR CONVENIENCE */
    /* TODO: THROW CONSTANTS IN DIFFERENT FILE SO IT'S PERSISTENT. */
    
    private readonly DatabaseDbContext _context;
    
    public PostService(DatabaseDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CreateAsync(CreatePostCommand command, string userId)
    {
        var title = command.Title;
        var body = command.Body;
        var tags = command.Tags;
        
        if (title.Length < PostConstraints.MinTitleLength) { return false; }
        if (title.Length > PostConstraints.MaxTitleLength) { return false; }

        if (body.Length > PostConstraints.MaxBodyLength) { return false; }
        
        if (tags.Count < PostConstraints.MinTagLength) { return false; }
        if (tags.Count > PostConstraints.MaxTagLength) { return false; }
        
        for (int i = 0; i < tags.Count; i++)
        {
            if (tags[i].Length < PostConstraints.MinTagNameLength )
            {
                return false;
            }
            
            if (tags[i].Length > PostConstraints.MaxTagNameLength )
            {
                return false;
            }
        }

        var posts = _context.Posts;
        var post = new Post
        {
            UserId = userId,
            Title = title,
            NormalizedTitle = title.ToUpper(),
            Body = body,
            Tags = tags,
            Done = true
        };
        posts.Add(post);
        await _context.SaveChangesAsync();
        return true;
    }
}