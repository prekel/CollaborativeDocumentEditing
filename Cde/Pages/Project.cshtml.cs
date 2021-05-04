using System.Collections.Generic;
using System.Threading.Tasks;

using Cde.Authorization;
using Cde.Data;
using Cde.Models;
using Cde.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cde.Pages
{
    [Authorize]
    public class ProjectModel : PageModel
    {
        private readonly ProjectService _projectService;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly IAuthorizationService _authService;

        public ProjectModel(ProjectService projectService, UserManager<ApplicationUser> userService,
            IAuthorizationService authService)
        {
            _projectService = projectService;
            _userService = userService;
            _authService = authService;
        }

        public ProjectViewModel Project { get; private set; } = null!;
        public ICollection<UpdateViewModel> Updates { get; private set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var p = await _projectService.GetProjectWithParticipants(id);
            var authResult = await _authService.AuthorizeAsync(User, p, nameof(IsProjectParticipant));
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            var user = await _userService.GetUserAsync(User);
            Project = new ProjectViewModel(p, user.Id == p.OwnerId);
            Updates = await _projectService.GetUpdatesWithDocumentInfo(id, await _userService.GetUserAsync(User));

            return Page();
        }

        [BindProperty]
        public CreateUpdateCommand? UpdateCommand { get; set; }

        public async Task<IActionResult> OnPostCommentAsync(int id)
        {
            if (UpdateCommand is null || !ModelState.IsValid)
            {
                return Page();
            }

            var p = await _projectService.GetProjectWithParticipants(id);
            var authResult = await _authService.AuthorizeAsync(User, p, nameof(IsProjectParticipant));
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            var user = await _userService.GetUserAsync(User);
            await _projectService.CreateUpdate(p.ProjectId, UpdateCommand, user);

            return RedirectToPage("/Project", id);
        }
    }
}
