using System.Collections.Generic;
using System.Net.Mime;
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
    public class ProjectModel : PageModel
    {
        private readonly IAuthorizationService _authService;
        private readonly ProjectService _projectService;
        private readonly UserManager<ApplicationUser> _userService;

        public ProjectModel(ProjectService projectService, UserManager<ApplicationUser> userService,
            IAuthorizationService authService)
        {
            _projectService = projectService;
            _userService = userService;
            _authService = authService;
        }

        public ProjectViewModel? Project { get; private set; }
        public ICollection<UpdateViewModel> Updates { get; private set; } = null!;

        public CommentInputModel CommentCommand { get; set; } = null!;

        public FileInputModel FileCommand { get; set; } = null!;

        public FileTextInputModel FileTextCommand { get; set; } = null!;


        public async Task<IActionResult> OnGetDocumentAsync(long id, long documentId)
        {
            var user = await _userService.GetUserAsync(User);
            var ar = await _projectService.IsUserHasAccess(id, user.Id);
            if (!ar)
            {
                return Forbid();
            }

            var doc = await _projectService.GetDocument(documentId);

            if (doc?.Blob is null)
            {
                return NotFound();
            }

            var blob = doc.Blob;

            var res = new FileContentResult(blob,
                doc.IsText
                    ? MediaTypeNames.Text.Plain
                    : MediaTypeNames.Application.Octet);

            return res;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userService.GetUserAsync(User);
            var ar = await _projectService.IsUserHasAccess(id, user.Id);
            if (!ar)
            {
                return Forbid();
            }

            Project = await _projectService.GetProjectViewModel(id, user.Id);
            Updates = await _projectService.GetUpdatesWithDocumentInfo(id, await _userService.GetUserAsync(User));

            return Page();
        }

        private async Task<IActionResult> ProceedCommand(long id, UpdateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                // return Page();
                return RedirectToPage("View", new {id});
            }

            var user = await _userService.GetUserAsync(User);
            var ar = await _projectService.IsUserHasAccess(id, user.Id);
            if (!ar)
            {
                return Forbid();
            }

            var update = await _projectService.CreateUpdate(id, inputModel, user);
            if (update is null)
            {
                return Forbid();
            }

            return RedirectToPage("View", new {id});
        }

        public async Task<IActionResult> OnPostCommentAsync(long id, CommentInputModel commentCommand) =>
            await ProceedCommand(id, commentCommand);

        public async Task<IActionResult> OnPostFileAsync(int id, FileInputModel fileCommand) =>
            await ProceedCommand(id, fileCommand);

        public async Task<IActionResult> OnPostFileTextAsync(int id, FileTextInputModel fileTextCommand) =>
            await ProceedCommand(id, fileTextCommand);

        public async Task<IActionResult> OnPostCloseAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("View", new {id});
            }

            var project = await _projectService.GetProject(id);
            if (project is null)
            {
                return RedirectToPage("View", new {id});
            }

            var user = await _userService.GetUserAsync(User);
            if (project.OwnerId != user.Id)
            {
                return Forbid();
            }

            await _projectService.CloseProject(id);

            return RedirectToPage("View", new {id});
        }
    }
}
