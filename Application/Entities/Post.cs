using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Common;

namespace Application.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string Title { get; set; }
    public string NormalizedTitle { get; set; }
    public string Body { get; set; }
    public List<string>? Tags { get; set; }
    public string DateCreated { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
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
    public List<DomainEvent> DomainEvents { get; }
}
public class PostCompletedEvent : DomainEvent
{
    public PostCompletedEvent(Post post)
    {
        Post = post;
    }
    
    public Post Post { get; }
}
