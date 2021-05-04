using System.ComponentModel.DataAnnotations;

namespace Cde.Models
{
    public record CreateUpdateCommand
    {
        [Required, StringLength(100)]
        public string CommentText { get; set; }
    }
}
