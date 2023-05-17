using Application.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Infrastructure.Persistence;

public class DatabaseDbContextSeed
{
    public static async Task SeedDefaultUserDataAsync(DatabaseDbContext context)
    {
        if (!context.Users.Any())
        {
            var hasher = new PasswordHasher<BlogUser>();

            var defaultUsers = new List<BlogUser>
            {
                new BlogUser { Id = "1", FirstName = "John", LastName = "Doe", UserName = "admin", Email = "admin@gmail.com", EmailConfirmed = true}
            };
            hasher.HashPassword(defaultUsers[0], "Password1!");
            await context.Users.AddRangeAsync(defaultUsers);
        }

        if (!context.UserRoles.Any())
        {
            await context.UserRoles.AddRangeAsync(new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> {RoleId = "1", UserId = "1"}
            });
        }
        await context.SaveChangesAsync();
    }
    
    public static async Task SeedDefaultRoleDataAsync(DatabaseDbContext context)
    {
        if (!context.Users.Any())
        {
            await context.Roles.AddRangeAsync(new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "2", Name = "Moderator", NormalizedName = "Moderator" },
                new IdentityRole { Id = "3", Name = "Member", NormalizedName = "Member" }
            });
        }
        await context.SaveChangesAsync();
    }
}