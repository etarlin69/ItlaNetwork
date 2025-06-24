using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El contenido del comentario no puede estar vacío.")]
        public string Content { get; set; }

        // Esta propiedad se llenará de forma oculta en el formulario
        // para saber a qué publicación pertenece el comentario.
        public int PostId { get; set; }
    }
}