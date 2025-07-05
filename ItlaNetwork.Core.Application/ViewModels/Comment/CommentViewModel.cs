using System;
using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Comment
{
    
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        
        public string UserId { get; set; }
        public string AuthorFullName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
    }
}