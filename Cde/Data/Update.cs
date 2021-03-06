using System;

namespace Cde.Data
{
    public record Update
    {
        public long UpdateId { get; set; }
        public string CommentText { get; set; } = null!;

        public DateTimeOffset CreateTimestamp { get; set; }

        public long? DocumentId { get; set; }
        public Document? Document { get; set; }

        public long ProjectId { get; set; }
        public Project? Project { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
    }
}
