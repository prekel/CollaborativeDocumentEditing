namespace Cde.Data
{
    public record Update
    {
        public long UpdateId { get; set; }
        public string CommentText { get; set; } = null!;

        public long? DocumentId { get; set; }
        public Document? Document { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
    }
}
