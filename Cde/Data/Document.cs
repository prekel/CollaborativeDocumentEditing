using System;

namespace Cde.Data
{
    public sealed record Document
    {
        public long DocumentId { get; set; }

        public bool IsText { get; set; }

        public string Filename { get; set; } = null!;

        public byte[] Blob { get; set; } = null!;

        public DateTimeOffset CreateTimestamp { get; set; }

        public Update? Update { get; set; }
    }
}
