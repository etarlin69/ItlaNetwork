using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ItlaNetwork.Infrastructure.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Verificar si los roles existen y si no, crearlos
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Basic"))
            {
                await roleManager.CreateAsync(new IdentityRole("Basic"));
            }
        }
    }
}