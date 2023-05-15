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
    
    // TAGS
    private const int MIN_TAGS = 1;
    private const int MAX_TAGS = 4;
    private const int MIN_TAG_NAME_LENGTH = 3;
    private const int MAX_TAG_NAME_LENGTH = 15;
    
    // TITLE
    private const int MIN_TITLE_LENGTH = 10;
    private const int MAX_TITLE_LENGTH = 50;
    
    // BODY
    private const int MAX_BODY_LENGTH = 30000;
    
    public PostService(DatabaseDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CustomCreateAsync(CreatePostCommand command, string userId)
    {
        var title = command.Title;
        var body = command.Body;
        var tags = command.Tags;
        
        if (title.Length < MIN_TITLE_LENGTH) { return false; }
        if (title.Length > MAX_TITLE_LENGTH) { return false; }

        if (body.Length > MAX_BODY_LENGTH) { return false; }
        
        if (tags.Count < MIN_TAGS) { return false; }
        if (tags.Count > MAX_TAGS) { return false; }
        for (int i = 0; i < tags.Count; i++)
        {
            if (tags[i].Length < MIN_TAG_NAME_LENGTH )
            {
                return false;
            }
            
            if (tags[i].Length > MAX_TAG_NAME_LENGTH )
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