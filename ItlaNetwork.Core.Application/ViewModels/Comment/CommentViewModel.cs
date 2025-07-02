using System;
using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Comment
{
    // ViewModel para crear o editar un comentario
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Datos del autor (a ser "cosidos" en la capa de lógica)
        public string UserId { get; set; }
        public string AuthorFullName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
    }
}
