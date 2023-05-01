using System.Reflection;
using Application.Common.Interface;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    private IConfiguration _configuration { get; set; }
    public IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }

    public IServiceCollection AddInfrastructure(IServiceCollection services)
    {
        services.AddDbContext<DatabaseDbContext>(
            options => options.UseNpgsql(_configuration.GetConnectionString("Postgres")));
        services.AddScoped<IDomainEventService, DomainEventService>();
        return services;
    }
}