using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Cde.Data;
using Cde.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cde.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ICollection<ProjectViewModel>> GetProjectsFor(ApplicationUser createdBy)
        {
            return await _dbContext.Projects
                .Include(p => p.InvitedParticipants)
                .Where(p => p.OwnerId == createdBy.Id || p.InvitedParticipants!.Any(ip => ip.Id == createdBy.Id))
                .Select(p1 => new ProjectViewModel(p1, p1.OwnerId == createdBy.Id))
                .ToListAsync();
        }

        public async Task<ICollection<UpdateViewModel>> GetUpdatesWithDocumentInfo(long projectId,
            ApplicationUser createdBy)
        {
            return await _dbContext.Updates
                .Include(u => u.Document)
                .Where(u => u.ProjectId == projectId)
                .Select(u => new UpdateViewModel(u, u.AuthorId == createdBy.Id))
                .ToListAsync();
        }

        public async Task<Project> GetProjectWithParticipants(long projectId)
        {
            return await _dbContext.Projects
                .Where(p => p.ProjectId == projectId)
                .Include(p => p.InvitedParticipants)
                .SingleAsync();
        }

        public async Task<ProjectViewModel> GetProjectInfo(long projectId, ApplicationUser createdBy)
        {
            return await _dbContext.Projects
                .Where(p => p.ProjectId == projectId)
                .Select(p1 => new ProjectViewModel(p1, p1.OwnerId == createdBy.Id))
                .SingleAsync();
        }

        public async Task<Document> CreateDocumentFromFileAsBlob(CreateUpdateCommand update)
        {
            if (update.DocumentFile is null)
            {
                throw new NullReferenceException(nameof(update.DocumentFile));
            }

            await using var memoryStream = new MemoryStream();
            await update.DocumentFile.CopyToAsync(memoryStream);

            // Upload the file if less than 5 MB
            if (memoryStream.Length >= 5 * 1024 * 2024)
            {
                throw new ApplicationException("File is larger than 5 MB");
            }

            var blob = memoryStream.ToArray();

            var d = new Document
            {
                // TODO IsText
                IsText = false,
                Filename = update.DocumentFile.FileName,
                Blob = blob
            };

            _dbContext.Add(d);
            await _dbContext.SaveChangesAsync();
            return d;
        }

        public async Task<Document> CreateDocumentFromTextAsBlob(CreateUpdateCommand update)
        {
            if (update.DocumentText is null)
            {
                throw new NullReferenceException(nameof(update.DocumentText));
            }

            var d = new Document
            {
                IsText = true,
                Filename = update.CommentText,
                Blob = System.Text.Encoding.UTF8.GetBytes(update.DocumentText)
            };
            _dbContext.Add(d);
            await _dbContext.SaveChangesAsync();
            return d;
        }

        public async Task<Update> CreateUpdate(long projectId, CreateUpdateCommand update,
            ApplicationUser createdBy)
        {
            var p = await _dbContext.Projects
                .Include(pr => pr.Updates)
                .SingleAsync(pr => pr.ProjectId == projectId);

            Document? d = null;
            if (update.DocumentFile is not null)
            {
                d = await CreateDocumentFromFileAsBlob(update);
            }
            else if (update.DocumentText is not null)
            {
                d = await CreateDocumentFromTextAsBlob(update);
            }

            var ret = new Update
            {
                CommentText = update.CommentText,
                DocumentId = d?.DocumentId,
                ProjectId = projectId,
                AuthorId = createdBy.Id,
            };
            p.Updates.Add(ret);
            await _dbContext.SaveChangesAsync();
            return ret;
        }
    }
}
