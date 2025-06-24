using ItlaNetwork.Core.Application.Enums; // <-- Añade este using
using ItlaNetwork.Core.Application.ViewModels.Comment;
using System;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy hh:mm tt");

        // Datos del autor
        public string UserId { get; set; }
        public string AuthorFullName { get; set; }
        public string AuthorUserName { get; set; }
        public string? AuthorProfilePictureUrl { get; set; }

        // --- PROPIEDADES AÑADIDAS PARA REACCIONES ---
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public ReactionType? CurrentUserReaction { get; set; } // Puede ser Like, Dislike o null

        // Comentarios
        public List<CommentViewModel> Comments { get; set; }
    }
}