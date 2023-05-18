using Application.Common.Manager;
using Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Infrastructure.Persistence;

public static class DatabaseDbModel
{
    public static void AutoRenameIdentityTables(ModelBuilder builder)
    {
        builder.Entity<BlogUser>(bu =>
        {
            bu.ToTable("users");
            
            bu.Property(u => u.UserName).HasMaxLength(255);
            bu.Property(u => u.Email).HasMaxLength(256);
            
            bu.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            bu.HasKey(r => r.Id);

            bu.HasMany<IdentityUserClaim<string>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            bu.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            bu.HasMany<IdentityUserToken<string>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            bu.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            bu.HasMany<Post>().WithOne().HasForeignKey(p => p.UserId).IsRequired();
        });
        
        builder.Entity<Post>(pt =>
        {
            pt.ToTable("posts");

            pt.HasKey(r => r.Id); 
            
            pt.HasIndex((p => new { p.Title, p.Id })).HasDatabaseName("PostIndex");

            pt.HasKey(p => p.Id);
        });
        
        builder.Entity<IdentityRole>(ir =>
        {
            ir.ToTable("roles");
            
            ir.HasKey(r => r.Id);
            ir.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

            ir.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            ir.Property(u => u.Name).HasMaxLength(256);
            ir.Property(u => u.NormalizedName).HasMaxLength(256);
            
            ir.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            ir.HasMany<IdentityRoleClaim<string>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });
        builder.Entity<IdentityRoleClaim<string>>(irc =>
        {
            irc.ToTable("role_claims");
            irc.HasKey(r => r.Id); 
        });

        builder.Entity<IdentityUserRole<string>>(iur =>
        {
            iur.ToTable("user_roles");
            iur.HasKey(r => new { r.UserId, r.RoleId }); 
        });
        builder.Entity<IdentityUserClaim<string>>(iuc =>
        {
            iuc.ToTable("user_claims");
            iuc.HasKey(uc => uc.Id); 
        });
        builder.Entity<IdentityUserLogin<string>>(iul =>
        {
            iul.ToTable("user_logins");
            iul.HasKey(l => new { l.LoginProvider, l.ProviderKey }); 
        });
        builder.Entity<IdentityUserToken<string>>(iut =>
        {
            iut.ToTable("user_tokens");
            iut.HasKey(t => new { t.UserId, t.LoginProvider, t.Name }); 
        });
    }
    public static void BaseUserModel(ModelBuilder builder, IConfiguration configuration)
    {
        var manager = new DefaultModelManager(configuration);
        var (adminUser, adminRole) =  manager.CreateDefaultModelAdminAccount(builder);
        
        builder.Entity<BlogUser>(e =>
        {
            e.Property(u => u.UserName).HasMaxLength(255);
            e.Property(u => u.Email).HasMaxLength(256);
            e.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            e.HasData(adminUser);
        });

        builder.Entity<IdentityUserRole<string>>(e =>
        {
            e.HasData(adminRole);
        });
    }

    public static void BaseRoleModel(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>(e =>
        {
            e.HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "2", Name = "Moderator", NormalizedName = "MODERATOR"},
                new IdentityRole { Id = "3", Name = "Member", NormalizedName = "MEMBER"}
            );
        });
    }
}