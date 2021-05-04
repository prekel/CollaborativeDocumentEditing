using Cde.Data;

namespace Cde.Models
{
    public record UpdateViewModel
    {
        public long UpdateId { get; set; }
        public string CommentText { get; set; }

        public DocumentInfoViewModel? DocumentInfo { get; set; }

        public long ProjectId { get; set; }

        public bool IsAuthor { get; set; }
        public string AuthorEmail { get; set; }

        public UpdateViewModel(Update update, bool isAuthor)
        {
            UpdateId = update.UpdateId;
            CommentText = update.CommentText;
            DocumentInfo = update.Document == null ? null : new DocumentInfoViewModel(update.Document);
            ProjectId = update.ProjectId;
            IsAuthor = isAuthor;
            AuthorEmail = update.Author.Email;
        }
    }
}
