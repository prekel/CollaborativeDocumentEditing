using System.ComponentModel.DataAnnotations;

namespace Cde.Models
{
    public record CreateInputModel
    {
        [Required]
        [StringLength(50)]
        public string ProjectName { get; set; }
    }
}
