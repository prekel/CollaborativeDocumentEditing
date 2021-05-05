using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Cde.Data;
using Cde.Models;
using Cde.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cde.Pages.Projects
{
    [Authorize]
    public class ProjectsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ProjectService _projectService;
        private readonly UserManager<ApplicationUser> _userService;

        public ProjectsModel(ApplicationDbContext dbContext, ProjectService projectService,
            UserManager<ApplicationUser> userService)
        {
            _dbContext = dbContext;
            _projectService = projectService;
            _userService = userService;
        }

        public ICollection<ProjectViewModel> Projects { get; private set; } = null!;

        public async Task OnGetAsync()
        {
            Projects = await _projectService.GetProjectViewModels(await _userService.GetUserAsync(User));
        }
    }
}