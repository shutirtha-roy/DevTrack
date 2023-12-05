using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DevTrack.Infrastructure.Securities
{
    public class ApiProjectListRequirementHandler : AuthorizationHandler<ApiProjectListRequirement>
    {
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               ApiProjectListRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == ClaimTypes.Sid))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}