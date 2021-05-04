using System;
using System.Collections;
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
        readonly ApplicationDbContext _dbContext;

        public ProjectService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ICollection<ProjectViewModel>> GetProjects(IdentityUser createdBy)
        {
            return await _dbContext.Projects.Select(p1 => new ProjectViewModel(p1)).ToListAsync();
        }
    }
}
