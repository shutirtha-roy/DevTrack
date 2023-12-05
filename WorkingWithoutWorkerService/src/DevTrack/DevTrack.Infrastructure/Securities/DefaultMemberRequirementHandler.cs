using Microsoft.AspNetCore.Authorization;

namespace DevTrack.Infrastructure.Securities
{
    public class DefaultMemberRequirementHandler : AuthorizationHandler<DefaultMemberRequirement>
    {
        private const string _createClaim = "CreateProject";
        private const string _editClaim = "EditProject";
        private const string _viewClaim = "ViewProject";
        private const string _deleteClaim = "DeleteProject";
        private const string _archiveClaim = "ArchiveProject";
        private const string _allowClaim = "true";
        private const string _adminRole = "Admin";

        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               DefaultMemberRequirement requirement)
        {
            if ((context.User.HasClaim(x => x.Type == _createClaim && x.Value == _allowClaim) &&
                context.User.HasClaim(x => x.Type == _editClaim && x.Value == _allowClaim) &&
                context.User.HasClaim(x => x.Type == _viewClaim && x.Value == _allowClaim) &&
                context.User.HasClaim(x => x.Type == _deleteClaim && x.Value == _allowClaim) &&
                context.User.HasClaim(x => x.Type == _archiveClaim && x.Value == _allowClaim)) ||
                context.User.IsInRole(_adminRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}