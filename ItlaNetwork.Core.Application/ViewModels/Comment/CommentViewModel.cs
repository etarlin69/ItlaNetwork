using System;

namespace ItlaNetwork.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Datos del autor del comentario
        public string AuthorFullName { get; set; }
        public string? AuthorProfilePictureUrl { get; set; }
    }
}