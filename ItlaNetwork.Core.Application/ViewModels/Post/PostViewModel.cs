using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Post
{
    /// <summary>
    /// ViewModel que representa una publicación para ser mostrada en la UI.
    /// Contiene toda la información necesaria para renderizar un "Post Card".
    /// </summary>
    public class PostViewModel
    {
        // --- Propiedades de la Publicación ---
        public int Id { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Propiedad computada que formatea la fecha de creación a un formato relativo y amigable.
        /// </summary>
        public string CreatedAtFormatted
        {
            get
            {
                var timeSpan = DateTime.Now - CreatedAt;
                if (timeSpan.TotalMinutes < 1) return "Hace un momento";
                if (timeSpan.TotalMinutes < 60) return $"Hace {timeSpan.Minutes} {(timeSpan.Minutes == 1 ? "minuto" : "minutos")}";
                if (timeSpan.TotalHours < 24) return $"Hace {timeSpan.Hours} {(timeSpan.Hours == 1 ? "hora" : "horas")}";
                if (timeSpan.TotalDays < 7) return $"Hace {timeSpan.Days} {(timeSpan.Days == 1 ? "día" : "días")}";
                return CreatedAt.ToString("dd MMM yyyy"); // Formato para fechas más antiguas
            }
        }

        // --- Datos del Autor (Poblados por el servicio) ---
        public string UserId { get; set; }
        public string AuthorFullName { get; set; }
        public string AuthorUserName { get; set; }
        public string? AuthorProfilePictureUrl { get; set; }

        // --- Datos de Reacciones y Comentarios (Poblados por el servicio) ---
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public ReactionType? CurrentUserReaction { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}