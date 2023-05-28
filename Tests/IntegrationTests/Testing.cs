using Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;

namespace Tests.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    
    private static IConfigurationRoot t_configuration = null!;
    private static IServiceScopeFactory t_scopeFactory = null!;
    private static Respawner t_checkpoint = null!;
    private static string? t_currentUserId;
    
    [OneTimeSetUp]
    public void RunThisFirstBeforeAnything()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();
        t_configuration = builder.Build();

        var startup = new Startup(t_configuration);
        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "SimpleBlog.Web"));
        
        services.AddLogging();
        
        startup.AddInfrastructure(services);
        startup.ConfigureServices(services);
        
        
        t_checkpoint = Respawner.CreateAsync(t_configuration.GetConnectionString("DefaultConnection")!, new RespawnerOptions
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        }).GetAwaiter().GetResult();

    }
    
    public static async Task ResetState()
    {
        try
        {
            await t_checkpoint.ResetAsync(t_configuration.GetConnectionString("DefaultConnection")!);
        }
        catch (Exception) 
        {
        }

        t_currentUserId = null;
    }
    
    public static string? GetCurrentUserId()
    {
        return t_currentUserId;
    }
    
    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}