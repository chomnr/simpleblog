using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Common;
using Newtonsoft.Json;

namespace Application.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public string? Username { get; set; }
    public string Title { get; set; }
    public string NormalizedTitle { get; set; }
    public string Body { get; set; }
    public List<string>? Tags { get; set; }
    public string DateCreated { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
}
public class PostCompletedEvent : DomainEvent
{
    public PostCompletedEvent(Post post)
    {
        Post = post;
    }
    
    public Post Post { get; }
}


public static class PostHelper
{
    public static Post CreateSimplifiedPost(string userId, string title, string body, List<string> tags)
    {
        return new Post
        {
            UserId = userId,
            Title = title,
            NormalizedTitle = title.ToUpper(),
            Body = body,
            Tags = tags
        };
    }
}