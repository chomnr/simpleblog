using System.Reflection;
using Application.Common;
using Application.Common.Interface;
using Application.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Infrastructure.Persistence;

/// <summary>
/// Database Type: POSTGRES
/// 
/// -----------------------------
/// Default DB Credentials:
/// --> Username: postgres
/// --> Password: Postgres
/// --> Port: 5432
/// 
/// -----------------------------
/// Default User Credentials
/// --> Username: admin 
/// --> Password: Password1!
/// --> Email: admin@domain.com
/// 
/// </summary>
public class DatabaseDbContext : IdentityDbContext<BlogUser>
{
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;
    private readonly IDomainEventService _domainEventService;
    
    public DatabaseDbContext(IConfiguration configuration,
        DbContextOptions<DatabaseDbContext> options,
        IMediator mediator,
        IDomainEventService domainEventService) : base(options)
    {
        _configuration = configuration;
        _mediator = mediator;
        _domainEventService = domainEventService;
    }
    
    public DbSet<Post> Posts => Set<Post>();
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        /*
        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();
        
        await DispatchEvents(events);
        */
        
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<DomainEvent>();
        
        DatabaseDbModel.AutoRenameIdentityTables(builder);
        DatabaseDbModel.BaseRoleModel(builder);
        DatabaseDbModel.BaseUserModel(builder, _configuration);
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}