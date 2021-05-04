using System.Collections.Generic;
using System.Linq;

namespace Cde.Models
{
    public record ProjectViewModel
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }

        public bool IsOwner { get; set; }

        public ProjectViewModel(Data.Project project, bool isOwner)
        {
            ProjectId = project.ProjectId;
            Name = project.Name;
            IsOwner = isOwner;
        }
    }
}
