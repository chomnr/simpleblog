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
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
}

public class BlogUserCompletedEvent : DomainEvent
{
    public BlogUserCompletedEvent(BlogUser account)
    {
        Account = account;
    }
    
    public BlogUser Account { get; }
}