using ItlaNetwork.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Identity.Seeds
{
    public static class IdentityDataSeeder
    {
        
        public static async Task SeedUsersAndRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Basic"))
            {
                await roleManager.CreateAsync(new IdentityRole("Basic"));
            }

            
            var defaultUser = new ApplicationUser
            {
                UserName = "adminuser",
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                Phone = "809-555-5555", 
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    
                    await userManager.CreateAsync(defaultUser, "Etarlin123");
                    
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                    await userManager.AddToRoleAsync(defaultUser, "Basic");
                }
            }
        }
    }
}