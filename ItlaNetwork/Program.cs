using System;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Application.Services;
using ItlaNetwork.Core.Application.Interfaces.Repositories;
using ItlaNetwork.Infrastructure.Persistence.Repositories;
using ItlaNetwork.Infrastructure.Shared;
using ItlaNetwork.Infrastructure.Identity;
using ItlaNetwork.Infrastructure.Persistence;
using ItlaNetwork.Core.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ItlaNetwork.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddPersistenceInfrastructure(configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(configuration);
builder.Services.AddIdentityInfrastructure(configuration);


builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IShipRepository, ShipRepository>();
builder.Services.AddScoped<IShipPositionRepository, ShipPositionRepository>();
builder.Services.AddScoped<IAttackRepository, AttackRepository>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ItlaNetwork.Infrastructure.Identity.Models.ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await ItlaNetwork.Infrastructure.Identity.Seeds.IdentityDataSeeder.SeedUsersAndRolesAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error durante el seeding de la base de datos.");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SyncSessionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
