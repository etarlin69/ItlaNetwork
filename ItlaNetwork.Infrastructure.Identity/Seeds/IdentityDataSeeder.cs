using ItlaNetwork.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Identity.Seeds
{
    public static class IdentityDataSeeder
    {
        // Este método se encargará de crear los roles y el usuario administrador por defecto.
        public static async Task SeedUsersAndRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. Crear los Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Basic"))
            {
                await roleManager.CreateAsync(new IdentityRole("Basic"));
            }

            // 2. Crear el Usuario Administrador por defecto
            var defaultUser = new ApplicationUser
            {
                UserName = "adminuser",
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                Phone = "809-555-5555", // Añadido para consistencia
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            // Verificar si el usuario administrador ya existe
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    // Crear el usuario con la contraseña
                    await userManager.CreateAsync(defaultUser, "Pa$$w0rd");
                    // Asignar los roles
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                    await userManager.AddToRoleAsync(defaultUser, "Basic");
                }
            }
        }
    }
}