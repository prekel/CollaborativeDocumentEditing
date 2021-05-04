using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Cde.Data;

namespace Cde.Authorization
{
    public class IsProjectParticipantHandler :
        AuthorizationHandler<IsProjectParticipant, Project>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IsProjectParticipantHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsProjectParticipant requirement,
            Project resource)
        {
            var appUser = await _userManager.GetUserAsync(context.User);
            if (appUser == null)
            {
                return;
            }

            if (resource.OwnerId == appUser.Id)
            {
                context.Succeed(requirement);
            }

            if (resource.InvitedParticipants != null && resource.InvitedParticipants.Any(p => p.Id == appUser.Id))
            {
                context.Succeed(requirement);
            }
        }
    }
}
