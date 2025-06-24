using ItlaNetwork.Core.Application;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Core.Domain.Settings;
using ItlaNetwork.Infrastructure.Persistence;
using ItlaNetwork.Infrastructure.Persistence.Contexts;
using ItlaNetwork.Infrastructure.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(builder.Configuration);

#region Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
#endregion

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSession();

// --- SECCIÓN MODIFICADA: Integramos la localización con MVC ---
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Hacemos que la ruta de los recursos sea "Resources"
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
// --- FIN DE LA SECCIÓN MODIFICADA ---

#if DEBUG
var mvcBuilder = builder.Services.AddControllersWithViews(); // Se mantiene para AddRazorRuntimeCompilation si lo usas
mvcBuilder.AddRazorRuntimeCompilation();
#endif


var app = builder.Build();

// Seeding...
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await ItlaNetwork.Infrastructure.Persistence.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
        await ItlaNetwork.Infrastructure.Persistence.Seeds.DefaultAdminUser.SeedAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante el seeding de la base de datos.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseErrorHandlerMiddleware();
    app.UseHsts();
}

// --- SECCIÓN MODIFICADA: Usamos una cultura más genérica ---
var supportedCultures = new[] { new CultureInfo("es") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});
// --- FIN DE LA SECCIÓN MODIFICADA ---

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();