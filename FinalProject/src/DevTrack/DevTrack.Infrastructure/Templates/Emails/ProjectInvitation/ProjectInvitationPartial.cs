namespace DevTrack.Infrastructure.Templates.Emails.ProjectInvitation
{
    public partial class ProjectInvitation
    {
        public string ProjectLink { get; set; }

        public ProjectInvitation(string projectLink)
        {
            ProjectLink = projectLink;
        }
    }
}