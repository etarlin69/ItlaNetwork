using ItlaNetwork.Core.Application;
using ItlaNetwork.Infrastructure.Persistence;
using ItlaNetwork.Infrastructure.Shared;
using ItlaNetwork.Infrastructure.Identity;
using ItlaNetwork.Infrastructure.Identity.Models;
using ItlaNetwork.Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Services required for session and user context.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();

// Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register all layers of the architecture
builder.Services.AddPersistenceInfrastructure(configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(configuration);
builder.Services.AddIdentityInfrastructure(configuration);

var app = builder.Build();

// Seeding logic
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await IdentityDataSeeder.SeedUsersAndRolesAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during the seeding of the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// MIDDLEWARE: El orden es crucial para el funcionamiento correcto.
app.UseAuthentication();
app.UseAuthorization();
app.UseSession(); // La sesión se configura después de la autenticación

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();