using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Domain.Enums; // Correct using for the enum
using System;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // --- FIX: Added 'CreatedAtFormatted' property ---
        // This property formats the date into a user-friendly, relative time string.
        public string CreatedAtFormatted
        {
            get
            {
                var timeSpan = DateTime.Now - CreatedAt;
                if (timeSpan.TotalMinutes < 1) return "Hace un momento";
                if (timeSpan.TotalMinutes < 60) return $"Hace {timeSpan.Minutes} {(timeSpan.Minutes == 1 ? "minuto" : "minutos")}";
                if (timeSpan.TotalHours < 24) return $"Hace {timeSpan.Hours} {(timeSpan.Hours == 1 ? "hora" : "horas")}";
                if (timeSpan.TotalDays < 7) return $"Hace {timeSpan.Days} {(timeSpan.Days == 1 ? "día" : "días")}";
                return CreatedAt.ToString("dd MMM yyyy");
            }
        }

        // Author data (to be "stitched" in the service layer)
        public string UserId { get; set; }
        public string AuthorFullName { get; set; }
        public string AuthorUserName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }

        // Reaction and comment data
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public ReactionType? CurrentUserReaction { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}