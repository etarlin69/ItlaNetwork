using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ItlaNetwork.Infrastructure.Identity.Contexts
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            var connectionString = configuration.GetConnectionString("IdentityConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.InvalidOperationException("Connection string 'IdentityConnection' not found.");
            }

            optionsBuilder.UseSqlServer(connectionString);
            return new IdentityContext(optionsBuilder.Options);
        }
    }
}