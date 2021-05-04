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


        private async Task<IActionResult> ProceedCommand(long projectId, UpdateCommand command)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Projects/Project", projectId);
            }

            var p = await _projectService.GetProjectWithParticipants(projectId);
            var authResult = await _authService.AuthorizeAsync(User, p, nameof(IsProjectParticipant));
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            var user = await _userService.GetUserAsync(User);
            await _projectService.CreateUpdate(p.ProjectId, command, user);

            return RedirectToPage("/Projects/Project", projectId);
        }


        // [BindProperty]
        public CommentCommand? CommentCommand { get; set; } = null!;

        public async Task<IActionResult> OnPostCommentAsync(long id, CommentCommand? CommentCommand)
        {
            return await ProceedCommand(id, CommentCommand);
        }

        // [BindProperty]
        public FileCommand? FileCommand { get; set; } = null!;

        public async Task<IActionResult> OnPostFileAsync(int id, FileCommand? FileCommand)
        {
            return await ProceedCommand(id, FileCommand);
        }

        // [BindProperty]
        public FileTextCommand? FileTextCommand { get; set; } = null!;

        public async Task<IActionResult> OnPostFileTextAsync(int id, FileTextCommand? FileTextCommand)
        {
            return await ProceedCommand(id, FileTextCommand);
        }
    }
}
