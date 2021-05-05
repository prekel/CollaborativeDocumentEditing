using System.Collections.Generic;
using System.Linq;

using Cde.Data;

namespace Cde.Models
{
    public record ProjectViewModel
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }

        public bool IsOwner { get; set; }
        
        public string Owner { get; set; }

        public ICollection<string> Participants { get; set; }

        public ProjectViewModel(Project project, bool isOwner)
        {
            ProjectId = project.ProjectId;
            Name = project.Name;
            IsOwner = isOwner;
            Owner = project.Owner.Email;
            Participants = project.InvitedParticipants.Select(p => p.Email).ToList();
        }
    }
}
