using System.Reflection;
using Application.Common.Interface;
using Application.Entities;
using Application.Features.BlogUser;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    private IConfiguration Configuration { get; set; }
    public IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddRouting(options => { options.LowercaseUrls = true; });
        services.AddMvc();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        services.AddScoped<IHostEnvironmentAuthenticationStateProvider>(sp =>
            (ServerAuthenticationStateProvider) sp.GetRequiredService<AuthenticationStateProvider>()
        );
        return services;
    }

    public IServiceCollection AddInfrastructure(IServiceCollection services)
    {
        services.AddDbContext<DatabaseDbContext>(
            options => options.UseNpgsql(Configuration.GetConnectionString("Postgres")));
        services.AddScoped<IDomainEventService, DomainEventService>();
        services.AddScoped<ICustomIdentityService, CustomIdentityService>();
        services.AddScoped<ICustomSignInService, CustomSignInService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IWebHelperService, WebHelperService>();
        services.AddScoped<SignInManager<BlogUser>>();
        return services;
    }
}