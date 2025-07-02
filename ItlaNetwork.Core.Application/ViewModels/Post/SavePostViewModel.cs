using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar el contenido de la publicación")]
        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        // --- CORRECCIÓN ---
        // Se vuelve a añadir la propiedad UserId, pero SIN el atributo [Required].
        // Esto permite que el modelo sea válido al crear un post (donde el UserId viene del servidor),
        // y que esté disponible para las comprobaciones de autorización.
        public string UserId { get; set; }
    }
}