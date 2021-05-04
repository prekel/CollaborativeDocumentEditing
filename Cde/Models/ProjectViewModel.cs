using System.Collections.Generic;
using System.Linq;

namespace Cde.Models
{
    public record ProjectViewModel
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }

        public bool IsOwner { get; set; }

        public ProjectViewModel(Data.Project project)
        {
            ProjectId = project.ProjectId;
            Name = project.Name;
        }
    }
}
