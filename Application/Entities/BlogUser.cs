using System.ComponentModel.DataAnnotations.Schema;
using Application.Common;
using Microsoft.AspNetCore.Identity;

namespace Application.Entities;

public class BlogUser : IdentityUser
{
    public string? Avatar { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int NameDisplayType { get; set; } = 1;
    
    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value && _done == false)
            {
                DomainEvents.Add(new BlogUserCompletedEvent(this));
            }

            _done = value;
        }
    }
    public List<DomainEvent> DomainEvents { get; }
}

public class BlogUserCompletedEvent : DomainEvent
{
    public BlogUserCompletedEvent(BlogUser account)
    {
        Account = account;
    }
    
    public BlogUser Account { get; }
}

[NotMapped]
public static class BlogUserHelper
{
    public static BlogUser CreateBlogUser(string firstName, string lastName, string userName, string password, string email, bool confirmEmail)
    {
        var hasher = new PasswordHasher<BlogUser>();
        var user = new BlogUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            NormalizedUserName = userName.ToUpper(),
            Email = email,
            NormalizedEmail = email.ToUpper(),
            EmailConfirmed = confirmEmail
        };
        user.PasswordHash = hasher.HashPassword(user, password);
        return user;
    }
}