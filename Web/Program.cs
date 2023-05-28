using Application;
using Application.Common;
using Application.Entities;
using Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Web.Areas.Account;
using Web.Areas.Account.Pages;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var application = new Startup(builder.Configuration);

application.AddInfrastructure(services);
application.ConfigureServices(services);

services.AddDatabaseDeveloperPageExceptionFilter();
services.AddDefaultIdentity<BlogUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
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

services.AddHttpContextAccessor();

builder.Services.AddAntiforgery();

services.AddRazorPages();
services.AddServerSideBlazor();
services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<BlogUser>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();