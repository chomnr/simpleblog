using Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Application.Common.Manager;

public class DefaultModelManager
{
    private readonly IConfiguration _configuration;

    public DefaultModelManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (BlogUser, IdentityUserRole<string>) CreateDefaultModelAdminAccount(ModelBuilder builder)
    {
        var admin = _configuration.GetSection("Authorization").GetSection("Admin");
        var defaultAdminUser = BlogUserHelper.CreateBlogUser(
            admin["FirstName"], 
            admin["LastName"], 
            admin["Username"], 
            admin["Password"], 
            admin["Email"]);
        var adminRole = new IdentityUserRole<string>
        {
            UserId = defaultAdminUser.Id,
            RoleId = "1"
        };
        return (defaultAdminUser, adminRole);
    }
}