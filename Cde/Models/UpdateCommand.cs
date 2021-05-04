using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace Cde.Models
{
    public record UpdateCommand
    {
        [Required, StringLength(100)]
        public string CommentText { get; set; }
    }


    public record FileCommand : UpdateCommand
    {
        public bool IsText { get; set; }

        public string? FileName { get; set; }

        [Required]
        public IFormFile DocumentFile { get; set; }
    }


    public record FileTextCommand : UpdateCommand
    {
        [Required]
        public string FileName { get; set; }

        public string DocumentText { get; set; }
    }


    public record CommentCommand : UpdateCommand
    {
    }
}
