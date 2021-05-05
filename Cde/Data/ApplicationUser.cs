using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace Cde.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Project>? InvitedProjects { get; set; }
    }
}
