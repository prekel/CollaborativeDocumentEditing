namespace Cde.Data
{
    public sealed record Document
    {
        public long DocumentId { get; set; }

        public bool IsText { get; set; }

        public string Filename { get; set; }

        public string? S3Link { get; set; }

        public byte[]? Blob { get; set; }

        public Update Update { get; set; }
    }
}
