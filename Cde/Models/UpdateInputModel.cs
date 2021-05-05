using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace Cde.Models
{
    public abstract record UpdateInputModel
    {
        [Required]
        [StringLength(100)]
        public string CommentText { get; set; }
    }


    public record FileInputModel : UpdateInputModel
    {
        public bool IsText { get; set; }

        public string? FileName { get; set; }

        [Required]
        public IFormFile DocumentFile { get; set; }
    }


    public record FileTextInputModel : UpdateInputModel
    {
        [Required]
        public string FileName { get; set; }

        public string DocumentText { get; set; }
    }


    public record CommentInputModel : UpdateInputModel
    {
    }
}
