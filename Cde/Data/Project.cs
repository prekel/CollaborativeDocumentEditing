using System;
using System.Collections.Generic;

namespace Cde.Data
{
    public sealed record Project
    {
        public long ProjectId { get; set; }
        public string Name { get; set; } = null!;

        public string OwnerId { get; set; } = null!;

        public bool IsClosed { get; set; }

        public DateTimeOffset CreateTimestamp { get; set; }

        public ApplicationUser? Owner { get; set; }

        public ICollection<ApplicationUser>? InvitedParticipants { get; set; }

        public ICollection<Update>? Updates { get; set; }
    }
}
