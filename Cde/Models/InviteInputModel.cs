using System.ComponentModel.DataAnnotations;

namespace Cde.Models
{
    public record InviteInputModel
    {
        [Required]
        [EmailAddress]
        public string NewParticipantEmailAddress { get; set; } = null!;
    }
}
