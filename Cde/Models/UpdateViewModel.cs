using System;

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

        public DateTimeOffset CreateTimestamp { get; set; }

        public UpdateViewModel(Update update, Document? document, string authorEmail, bool isAuthor)
        {
            UpdateId = update.UpdateId;
            CommentText = update.CommentText;
            DocumentInfo = document == null ? null : new DocumentInfoViewModel(document);
            ProjectId = update.ProjectId;
            IsAuthor = isAuthor;
            AuthorEmail = authorEmail;
            CreateTimestamp = update.CreateTimestamp;
        }
    }
}
