using System.Reflection;
using Application.Common;
using Application.Entities;
using MediatR;
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
    private readonly IMediator _mediator;
    

    public DatabaseDbContext(IConfiguration configuration,
        DbContextOptions<DatabaseDbContext> options,
        IMediator mediator) : base(options)
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    //public DbSet<BlogUser> BlogUsers => Set<BlogUser>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Ignore<DomainEvent>();

        builder.Entity<Post>(e =>
        {
            e.ToTable("posts");

            var testPost = new Post
            {
                PostId = "1",
                UserId = "1",
                Title = "First Blog Post",
                NormalizedTitle = "FIRST BLOG POST",
                Body = "You've successfully setup SimpleBlog",
                Tags = new List<string> {"Introduction", "Welcoming", "Cool"}
            };

            e.HasData(testPost);

            e.Property(p => p.PostId).HasIdentityOptions(startValue: 1, incrementBy: 1);
            
            e.HasIndex((p => new { p.Title, p.PostId })).HasDatabaseName("PostIndex");

            e.HasKey(p => p.PostId);
        });
         builder.Entity<BlogUser>(e =>
        {
            e.ToTable("users");
            
            e.Property(u => u.UserName).HasMaxLength(255);
            e.Property(u => u.Email).HasMaxLength(256);
            
            e.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            
            /* Default Admin User */
            var hasher = new PasswordHasher<BlogUser>();
            var adminAccount = new BlogUser
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@nowhere.land",
                NormalizedEmail = "ADMIN@NOWHERE.LAND",
                EmailConfirmed = true,
                Done = true
            };
            adminAccount.PasswordHash = hasher.HashPassword(adminAccount, "Password1!");
            e.HasData(adminAccount);

            e.HasMany<IdentityUserClaim<string>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            e.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            e.HasMany<IdentityUserToken<string>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            e.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            e.HasMany<Post>().WithOne().HasForeignKey(p => p.UserId).IsRequired();
        });
        
        builder.Entity<IdentityRole>(e =>
        {
            e.ToTable("roles");
            
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
            e.ToTable("user_roles");
            
            e.HasKey(r => new { r.UserId, r.RoleId });
            e.HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = "1",
            });
        });
        
        builder.Entity<IdentityUserClaim<string>>(e =>
        {
            e.ToTable("user_claims");
            
            e.HasKey(uc => uc.Id);
        });
        
        builder.Entity<IdentityUserLogin<string>>(e =>
        {
            e.ToTable("user_logins");
            
            e.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });
        
        builder.Entity<IdentityRoleClaim<string>>(e =>
        {
            e.ToTable("role_claims");
            
            e.HasKey(r => r.Id);
        });
        
        builder.Entity<IdentityUserToken<string>>(e =>
        {
            e.ToTable("user_tokens");
            
            e.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        });
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}