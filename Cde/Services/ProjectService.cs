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

        public async Task<ICollection<ProjectViewModel>> GetProjectViewModels(ApplicationUser createdBy)
        {
            return await _dbContext.Projects
                .Include(p => p.Owner)
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

        private async Task<Document> CreateDocumentFromFileAsBlob(FileInputModel fileInputModel)
        {
            if (fileInputModel.DocumentFile is null)
            {
                throw new NullReferenceException(nameof(fileInputModel.DocumentFile));
            }

            await using var memoryStream = new MemoryStream();
            await fileInputModel.DocumentFile.CopyToAsync(memoryStream);

            // Upload the file if less than 5 MB
            if (memoryStream.Length >= 5 * 1024 * 1024)
            {
                throw new ApplicationException("File is larger than 5 MB");
            }

            var blob = memoryStream.ToArray();

            var d = new Document
            {
                IsText = fileInputModel.IsText,
                Filename = fileInputModel.FileName ?? fileInputModel.DocumentFile.FileName,
                Blob = blob
            };

            _dbContext.Add(d);
            await _dbContext.SaveChangesAsync();
            return d;
        }

        private async Task<Document> CreateDocumentFromTextAsBlob(FileTextInputModel textInputModel)
        {
            if (textInputModel.DocumentText is null)
            {
                throw new NullReferenceException(nameof(textInputModel.DocumentText));
            }

            var d = new Document
            {
                IsText = true,
                Filename = textInputModel.FileName,
                Blob = System.Text.Encoding.UTF8.GetBytes(textInputModel.DocumentText)
            };
            _dbContext.Add(d);
            await _dbContext.SaveChangesAsync();
            return d;
        }

        public async Task<Update> CreateUpdate(long projectId, UpdateInputModel updateInputModel,
            ApplicationUser createdBy)
        {
            var p = await _dbContext.Projects
                .Include(pr => pr.Updates)
                .SingleAsync(pr => pr.ProjectId == projectId);

            var d = updateInputModel switch
            {
                FileInputModel fileCommand => await CreateDocumentFromFileAsBlob(fileCommand),
                FileTextInputModel textCommand => await CreateDocumentFromTextAsBlob(textCommand),
                _ => null
            };

            var ret = new Update
            {
                CommentText = updateInputModel.CommentText,
                DocumentId = d?.DocumentId,
                ProjectId = projectId,
                AuthorId = createdBy.Id,
            };
            p.Updates.Add(ret);
            await _dbContext.SaveChangesAsync();
            return ret;
        }


        public abstract record InviteParticipantResult
        {
            public record Ok : InviteParticipantResult;

            public record AlreadyInvited : InviteParticipantResult;

            public record UserNotExists : InviteParticipantResult;

            public record ProjectNotExists : InviteParticipantResult;
        }

        public async Task<InviteParticipantResult> InviteParticipant(long projectId, string invitedEmail)
        {
            var project = await _dbContext.Projects
                .Include(p => p.InvitedParticipants)
                .SingleOrDefaultAsync(p => p.ProjectId == projectId);
            if (project?.InvitedParticipants is null)
            {
                return new InviteParticipantResult.ProjectNotExists();
            }

            var invited = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Email == invitedEmail);
            if (invited is null)
            {
                return new InviteParticipantResult.UserNotExists();
            }

            var participants = project.InvitedParticipants;
            if (participants.Any(p => p.Email == invitedEmail))
            {
                return new InviteParticipantResult.AlreadyInvited();
            }

            participants.Add(invited);
            await _dbContext.SaveChangesAsync();
            return new InviteParticipantResult.Ok();
        }

        public async Task<ProjectViewModel?> GetProjectViewModel(long projectId, string queriedById)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Owner)
                .Include(p => p.InvitedParticipants)
                .SingleOrDefaultAsync(p => p.ProjectId == projectId);
            return project is null ? null : new ProjectViewModel(project, project.OwnerId == queriedById);
        }
    }
}
