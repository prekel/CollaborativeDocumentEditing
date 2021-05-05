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

        [BindProperty]
        public CreateInputModel? CreateInputModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || CreateInputModel is null)
            {
                return RedirectToPage();
            }

            var user = await _userService.GetUserAsync(User);

            var project = await _projectService.CreateProject(user, CreateInputModel);

            return RedirectToPage("View", new {id = project.ProjectId});
        }
    }
}
