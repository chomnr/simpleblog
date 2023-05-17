using Application.Common.Manager;
using Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Infrastructure.Persistence;

public static class DatabaseDbModel
{
    
    [Obsolete]
    public static void AutoRenameIdentityTables(ModelBuilder builder)
    {
        builder.Entity<BlogUser>(bu => { bu.HasKey(r => r.Id); bu.ToTable("users"); });
        
        builder.Entity<Post>(pt => { pt.HasKey(r => r.Id); pt.ToTable("posts"); });
        
        builder.Entity<IdentityRole>(ir => { ir.HasKey(r => r.Id); ir.ToTable("roles"); });
        builder.Entity<IdentityRoleClaim<string>>(irc => { irc.HasKey(r => r.Id); irc.ToTable("role_claims"); });

        builder.Entity<IdentityUserRole<string>>(iur => { iur.HasKey(r => new { r.UserId, r.RoleId }); iur.ToTable("user_roles"); });
        builder.Entity<IdentityUserClaim<string>>(iuc => { iuc.HasKey(uc => uc.Id); iuc.ToTable("user_claims"); });
        builder.Entity<IdentityUserLogin<string>>(iul => { iul.HasKey(l => new { l.LoginProvider, l.ProviderKey }); iul.ToTable("user_logins"); });
        builder.Entity<IdentityUserToken<string>>(iut => { iut.HasKey(t => new { t.UserId, t.LoginProvider, t.Name }); iut.ToTable("user_tokens"); });
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
            
            /*
            e.HasMany<IdentityUserClaim<string>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            e.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            e.HasMany<IdentityUserToken<string>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            e.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            e.HasMany<Post>().WithOne().HasForeignKey(p => p.UserId).IsRequired();
            */
        });

        builder.Entity<IdentityUserRole<string>>(e =>
        {
            e.HasData(adminRole);
        });
    }
}