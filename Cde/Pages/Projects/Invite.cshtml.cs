using System.Threading.Tasks;

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
    public class InviteModel : PageModel
    {
        private readonly IAuthorizationService _authService;
        private readonly ProjectService _projectService;
        private readonly UserManager<ApplicationUser> _userService;

        public InviteModel(ProjectService projectService, UserManager<ApplicationUser> userService,
            IAuthorizationService authService)
        {
            _projectService = projectService;
            _userService = userService;
            _authService = authService;
        }

        [BindProperty]
        public InviteInputModel? InviteInputModel { get; set; }

        public string? InviteParticipantResultMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var project = await _projectService.GetProject(id);
            if (project is null)
            {
                return RedirectToPage("/Projects");
            }

            var user = await _userService.GetUserAsync(User);
            if (project.OwnerId != user.Id)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid || InviteInputModel is null)
            {
                return Page();
                //return RedirectToPage("/Projects/Project", id);
            }

            var project = await _projectService.GetProject(id);
            if (project is null)
            {
                return RedirectToPage("/Projects");
            }

            var user = await _userService.GetUserAsync(User);
            if (project.OwnerId != user.Id)
            {
                return Forbid();
            }

            var inviteParticipantResult =
                await _projectService.InviteParticipant(id, InviteInputModel.NewParticipantEmailAddress);


            switch (inviteParticipantResult)
            {
                case ProjectService.InviteParticipantResult.Ok:
                    return RedirectToPage("View", new {id});
                case ProjectService.InviteParticipantResult.AlreadyInvited:
                    InviteParticipantResultMessage = "User already invited";
                    break;
                case ProjectService.InviteParticipantResult.UserNotExists:
                    InviteParticipantResultMessage = "User not exists";
                    break;
            }

            return Page();
        }
    }
}
