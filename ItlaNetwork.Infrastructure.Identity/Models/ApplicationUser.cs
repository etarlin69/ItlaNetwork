using Microsoft.AspNetCore.Identity;

namespace ItlaNetwork.Infrastructure.Identity.Models
{
    
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}