using Microsoft.AspNetCore.Http; // <--- Se añade este using
using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar el contenido de la publicación")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        // Se añade la propiedad para recibir el archivo del formulario
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        // Las propiedades UserId y CreatedAt se manejarán en el backend, no aquí.
    }
}