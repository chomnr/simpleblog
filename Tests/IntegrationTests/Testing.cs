using Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.IntegrationTests;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot t_configuration = null!;
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

        startup.ConfigureServices(services);
        
    }
    
    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}