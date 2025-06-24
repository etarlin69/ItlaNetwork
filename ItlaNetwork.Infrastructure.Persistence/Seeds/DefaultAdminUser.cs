using ItlaNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Persistence.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Datos del usuario administrador por defecto
            var defaultUser = new User
            {
                UserName = "adminuser",
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            // Verificar si el usuario ya existe
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    // Crear el usuario con la contraseña
                    // IMPORTANTE: Cambia esta contraseña por una segura en un entorno real
                    await userManager.CreateAsync(defaultUser, "Pa$$w0rd");

                    // Asignar los roles de "Admin" y "Basic"
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                    await userManager.AddToRoleAsync(defaultUser, "Basic");
                }
            }
        }
    }
}