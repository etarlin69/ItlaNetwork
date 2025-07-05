using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Account
{
    public class ProfileViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required, Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required, Phone, Display(Name = "Teléfono")]
        public string Phone { get; set; }

        
        [Display(Name = "Foto actual")]
        public string ProfileImageUrl { get; set; }

        
        [DataType(DataType.Upload)]
        [Display(Name = "Subir nueva foto")]
        public IFormFile ImageFile { get; set; }
    }
}
