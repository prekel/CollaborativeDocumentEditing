using System;
using System.Collections.Generic;

using Cde.Data;

namespace Cde.Models
{
    public record ProjectViewModel
    {
        public ProjectViewModel(Project project, string ownerEmail, ICollection<string> participantEmails, bool isOwner, int updatesCount)
        {
            ProjectId = project.ProjectId;
            Name = project.Name;
            IsClosed = project.IsClosed;
            IsOwner = isOwner;
            OwnerEmail = ownerEmail;
            ParticipantEmails = participantEmails;
            CreateTimestamp = project.CreateTimestamp;
            UpdatesCount = updatesCount;
        }

        public long ProjectId { get; set; }
        public string Name { get; set; }

        public bool IsOwner { get; set; }

        public string OwnerEmail { get; set; }

        public bool IsClosed { get; set; }
        
        public int UpdatesCount { get; set; }

        public DateTimeOffset CreateTimestamp { get; set; }

        public ICollection<string> ParticipantEmails { get; set; }
    }
}
