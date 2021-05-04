using System.Collections.Generic;
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

        public async Task<ICollection<ProjectViewModel>> GetProjects(ApplicationUser createdBy)
        {
            return await _dbContext.Projects
                .Select(p1 => new ProjectViewModel(p1, p1.OwnerId == createdBy.Id))
                .ToListAsync();
        }

        public async Task<ICollection<UpdateViewModel>> GetUpdates(long projectId, ApplicationUser createdBy)
        {
            return await _dbContext.Updates
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

        public async Task CreateUpdate(long projectId, CreateUpdateCommand update, ApplicationUser createdBy)
        {
            var p = await _dbContext.Projects
                .Include(pr => pr.Updates)
                .SingleAsync(pr => pr.ProjectId == projectId);
            p.Updates.Add(new Update()
            {
                CommentText = update.CommentText,
                DocumentId = null,
                ProjectId = projectId,
                AuthorId = createdBy.Id
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
