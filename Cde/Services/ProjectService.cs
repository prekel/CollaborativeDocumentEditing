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

        public async Task<Document> CreateDocumentFromFileAsBlob(FileCommand fileCommand)
        {
            if (fileCommand.DocumentFile is null)
            {
                throw new NullReferenceException(nameof(fileCommand.DocumentFile));
            }

            await using var memoryStream = new MemoryStream();
            await fileCommand.DocumentFile.CopyToAsync(memoryStream);

            // Upload the file if less than 5 MB
            if (memoryStream.Length >= 5 * 1024 * 1024)
            {
                throw new ApplicationException("File is larger than 5 MB");
            }

            var blob = memoryStream.ToArray();

            var d = new Document
            {
                IsText = (bool) fileCommand.IsText,
                Filename = fileCommand.FileName ?? fileCommand.DocumentFile.FileName,
                Blob = blob
            };

            _dbContext.Add(d);
            await _dbContext.SaveChangesAsync();
            return d;
        }

        public async Task<Document> CreateDocumentFromTextAsBlob(FileTextCommand textCommand)
        {
            if (textCommand.DocumentText is null)
            {
                throw new NullReferenceException(nameof(textCommand.DocumentText));
            }

            var d = new Document
            {
                IsText = true,
                Filename = textCommand.FileName,
                Blob = System.Text.Encoding.UTF8.GetBytes(textCommand.DocumentText)
            };
            _dbContext.Add(d);
            await _dbContext.SaveChangesAsync();
            return d;
        }

        public async Task<Update> CreateUpdate(long projectId, UpdateCommand updateCommand,
            ApplicationUser createdBy)
        {
            var p = await _dbContext.Projects
                .Include(pr => pr.Updates)
                .SingleAsync(pr => pr.ProjectId == projectId);

            var d = updateCommand switch
            {
                FileCommand fileCommand => await CreateDocumentFromFileAsBlob(fileCommand),
                FileTextCommand textCommand => await CreateDocumentFromTextAsBlob(textCommand),
                _ => null
            };

            var ret = new Update
            {
                CommentText = updateCommand.CommentText,
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
