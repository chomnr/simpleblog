using Application.Common;
using Application.Entities;
using Microsoft.AspNetCore.Identity;
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

    public DatabaseDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<BlogUser> BlogUsers => Set<BlogUser>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<DomainEvent>();
        builder.Entity<BlogUser>(e =>
        {
            //do stuff here.,.. configure default account, and limit characterts and such...
        });
        builder.Entity<IdentityRole>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
            
            // Default Roles
            e.HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "2", Name = "Member", NormalizedName = "MEMBER"}
            );

            e.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            
            e.Property(u => u.Name).HasMaxLength(256);
            e.Property(u => u.NormalizedName).HasMaxLength(256);
            
            e.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            e.HasMany<IdentityRoleClaim<string>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });

        builder.Entity<IdentityUserRole<string>>(e =>
        {
            e.HasKey(r => new { r.UserId, r.RoleId });
            e.HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = "1",
            });
        });
        
        builder.Entity<IdentityUserClaim<string>>(e =>
        {
            e.HasKey(uc => uc.Id);
        });
        
        builder.Entity<IdentityUserLogin<string>>(e =>
        {
            e.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });
        
        builder.Entity<IdentityRoleClaim<string>>(e =>
        {
            e.HasKey(r => r.Id);
        });
        
        builder.Entity<IdentityUserToken<string>>(e =>
        {
            e.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        });
    }
}