using Application;
using Application.Common;
using Application.Entities;
using Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Areas.Account;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var application = new Startup(builder.Configuration);

application.AddInfrastructure(services);
application.ConfigureServices(services);

services.AddDatabaseDeveloperPageExceptionFilter();
services.AddDefaultIdentity<BlogUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // enable true for email.
        options.SignIn.RequireConfirmedEmail = false;
        options.User.RequireUniqueEmail = true;
        
        options.User.AllowedUserNameCharacters = UsernameConstraints.AllowedCharacters;

        options.Password.RequireUppercase = PasswordConstraints.RequireUpperCase;
        options.Password.RequireNonAlphanumeric = PasswordConstraints.RequireNonAlphanumeric;
        options.Password.RequireDigit = PasswordConstraints.RequireDigit;
        options.Password.RequiredLength = PasswordConstraints.MinLength;
    })
    .AddEntityFrameworkStores<DatabaseDbContext>();

services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

services.AddServerSideBlazor().AddHubOptions(hub => hub.MaximumReceiveMessageSize = 100 * 1024 * 1024); 

services.AddHttpContextAccessor();

services.AddAntiforgery();

services.AddRazorPages();
services.AddServerSideBlazor();
services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<BlogUser>>();

var app = builder.Build();

// Must use if you have a Pathbase 
// Regular domain -> example.com || subdomain.example.com
// Domain w/ Pathbase -> example.com/simpleblog || test.example.com/simpleblog
//app.UsePathBase("/simpleblog");

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    //not necessary if cloudflare or nginx does it for you
    app.UseHttpsRedirection(); 
    //app.UseHsts();
    //app.UseMvc();
}
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//app.UseExceptionHandler("/Error");
//app.UseHsts();    Console.WriteLine("Running SimpleBlog in Production.");

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  

app.UseAuthorization();

app.Run();