using Microsoft.AspNetCore.Identity;

namespace DevTrack.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, IEntity<Guid>
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public List<ProjectUser>? UserProjects { get; set; }
        public string? TimeZone { get; set; }
    }
}