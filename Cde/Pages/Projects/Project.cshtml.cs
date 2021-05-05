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

namespace Cde.Pages.Projects
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

        public ProjectViewModel? Project { get; private set; } = null!;
        public ICollection<UpdateViewModel> Updates { get; private set; } = null!;

        private async Task GetProjectUpdates(long id, Project p, ApplicationUser user)
        {
            Project = new ProjectViewModel(p, user.Id == p.OwnerId);
            Updates = await _projectService.GetUpdatesWithDocumentInfo(id, await _userService.GetUserAsync(User));
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            var p = await _projectService.GetProjectWithParticipants(id);
            var authResult = await _authService.AuthorizeAsync(User, p, nameof(IsProjectParticipantOrOwner));
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            var user = await _userService.GetUserAsync(User);

            await GetProjectUpdates(id, p, user);

            return Page();
        }

        private async Task<IActionResult> ProceedCommand(long id, UpdateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Projects/Project", id);
            }

            var p = await _projectService.GetProjectWithParticipants(id);
            var authResult = await _authService.AuthorizeAsync(User, p, nameof(IsProjectParticipantOrOwner));
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            var user = await _userService.GetUserAsync(User);
            await _projectService.CreateUpdate(p.ProjectId, inputModel, user);

            return RedirectToPage("/Projects/Project", id);
        }

        public CommentInputModel CommentCommand { get; set; } = null!;

        public async Task<IActionResult> OnPostCommentAsync(long id, CommentInputModel commentCommand)
        {
            return await ProceedCommand(id, commentCommand);
        }

        public FileInputModel FileCommand { get; set; } = null!;

        public async Task<IActionResult> OnPostFileAsync(int id, FileInputModel fileCommand)
        {
            return await ProceedCommand(id, fileCommand);
        }

        public FileTextInputModel FileTextCommand { get; set; } = null!;

        public async Task<IActionResult> OnPostFileTextAsync(int id, FileTextInputModel fileTextCommand)
        {
            return await ProceedCommand(id, fileTextCommand);
        }

        public InviteInputModel InviteInputModel { get; set; }

        public ProjectService.InviteParticipantResult? InviteParticipantResult { get; set; }

        public async Task<IActionResult> OnPostInviteAsync(int id, InviteInputModel inviteInputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Projects/Project", id);
            }

            var p = await _projectService.GetProjectWithParticipants(id);
            var user = await _userService.GetUserAsync(User);
            if (p.OwnerId != user.Id)
            {
                return new ForbidResult();
            }

            InviteParticipantResult =
                await _projectService.InviteParticipant(id, inviteInputModel.NewParticipantEmailAddress);

            await GetProjectUpdates(id, p, user);

            return Page();
        }
    }
}
