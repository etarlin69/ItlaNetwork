using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El comentario no puede estar vacío")]
        public string Content { get; set; }

        // Estos campos sí son necesarios y vienen del formulario.
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        // --- CORRECCIÓN ---
        // La propiedad UserId ha sido eliminada. El servicio se encarga de obtenerla
        // del usuario autenticado, por lo que no debe ser parte del ViewModel del formulario.
    }
}