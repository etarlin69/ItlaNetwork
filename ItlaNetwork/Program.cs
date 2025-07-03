using System;
using ItlaNetwork.Core.Application;
using ItlaNetwork.Infrastructure.Persistence;
using ItlaNetwork.Infrastructure.Shared;
using ItlaNetwork.Infrastructure.Identity;
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

// 1) Añade MVC
builder.Services.AddControllersWithViews();

// 2) AutoMapper: escanea todos los perfiles en los ensamblados cargados
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 3) Servicios para sesión y contexto de usuario
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4) Registra las capas de Onion
builder.Services.AddPersistenceInfrastructure(configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(configuration);
builder.Services.AddIdentityInfrastructure(configuration);

var app = builder.Build();

// 5) Seeding de usuarios/roles
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

// 6) Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// --- CORRECCIÓN CRÍTICA EN EL ORDEN DEL MIDDLEWARE ---
// La sesión DEBE registrarse ANTES de la autenticación y autorización
// para que los datos del usuario estén disponibles cuando se validen las credenciales.
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SyncSessionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();