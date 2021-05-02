using System.Collections.Generic;

namespace Cde.Data
{
    public sealed record Project
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }

        public ICollection<Update> Updates { get; set; }
    }
}
