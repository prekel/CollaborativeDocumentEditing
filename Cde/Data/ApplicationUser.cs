using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace Cde.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Project> InvitedProjects { get; set; } = null!;
    }
}
