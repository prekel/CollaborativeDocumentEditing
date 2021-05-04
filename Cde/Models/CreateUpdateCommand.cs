using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace Cde.Models
{
    public record CreateUpdateCommand
    {
        [Required, StringLength(100)]
        public string CommentText { get; set; }

        public string? DocumentText { get; set; }

        public IFormFile? DocumentFile { get; set; }
    }
}
