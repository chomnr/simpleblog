using Application.Entities;
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
}