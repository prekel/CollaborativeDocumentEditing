using Cde.Data;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cde.Pages
{
    public class ProjectModel : PageModel
    {
        public Project? Project { get; private set; }

        public void OnGet(int id)
        {
            Project = new Project
            {
                ProjectId = id,
                Name = $"Project {id}"
            };
        }
    }
}
