using System;

using Cde.Data;

namespace Cde.Models
{
    public record DocumentInfoViewModel
    {
        public DocumentInfoViewModel(Document document)
        {
            DocumentId = document.DocumentId;
            IsText = document.IsText;
            Filename = document.Filename;
            CreateTimestamp = document.CreateTimestamp;
        }

        public long DocumentId { get; set; }

        public bool IsText { get; set; }

        public string Filename { get; set; } = null!;

        public DateTimeOffset CreateTimestamp { get; set; }
    }
}
