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
    public class CreateModel : PageModel
    {
        private readonly ProjectService _projectService;
        private readonly UserManager<ApplicationUser> _userService;

        public CreateModel(ProjectService projectService, UserManager<ApplicationUser> userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        public CreateFileInputModel? CreateFileInputModel { get; set; }

        public CreateFileTextInputModel? CreateFileTextInputModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostFileAsync(CreateFileInputModel createFileInputModel) =>
            await ProceedCreate(createFileInputModel);

        public async Task<IActionResult> OnPostFileTextAsync(CreateFileTextInputModel createFileTextInputModel) =>
            await ProceedCreate(createFileTextInputModel);

        private async Task<IActionResult> ProceedCreate(UpdateInputModel initial)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            var user = await _userService.GetUserAsync(User);

            var project = await _projectService.CreateProject(user, (ICreateFileInputModel) initial);

            await _projectService.CreateUpdate(project.ProjectId, initial, user);

            return RedirectToPage("View", new {id = project.ProjectId});
        }
    }
}
