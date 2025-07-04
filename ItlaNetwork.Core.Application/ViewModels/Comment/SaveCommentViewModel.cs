﻿using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El comentario no puede estar vacío")]
        public string Content { get; set; }

        
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        
    }
}