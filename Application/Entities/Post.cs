using Application.Common;

namespace Application.Entities;

public class Post
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? UserId { get; set; }
    public string Title { get; set; }
    public string NormalizedTitle { get; set; }
    public string Body { get; set; }
    public string? Category { get; set; }
    public string DateCreated { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
    public List<string>? Tags { get; set; }
    private bool _done;
    
    public bool Done
    {
        get => _done;
        set
        {
            if (value && _done == false)
            {
                DomainEvents.Add(new PostCompletedEvent(this));
            }

            _done = value;
        }
    }
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
}
public class PostCompletedEvent : DomainEvent
{
    public PostCompletedEvent(Post post)
    {
        Post = post;
    }
    
    public Post Post { get; }
}
