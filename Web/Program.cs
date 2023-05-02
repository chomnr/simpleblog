using Application;
using Application.Entities;
using Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Identity;
using Web.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var application = new Startup(builder.Configuration);

application.ConfigureServices(services);
application.AddInfrastructure(services);

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddIdentity<BlogUser, IdentityRole>(options => 
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<DatabaseDbContext>();
services.AddAntiforgery();

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
  
/*
app.UseEndpoints(endpoints =>  
{  
    endpoints.MapControllerRoute(  
        name: "default",  
        pattern: "{controller=Home}/{action=Index}/{id?}");  
});  */

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();