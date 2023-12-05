using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.DbContexts
{
    public interface IApplicationDbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<KeyboardActivities> KeyboardActivities { get; set; }
        public DbSet<RunningProgram> RunningPrograms { get; set; }
        public DbSet<WebcamCapture> WebcamCaptures { get; set; }
        public DbSet<ScreenCapture> ScreenCaptures { get; set; }
        public DbSet<ActiveWindows> ActiveWindows { get; set; }
        public DbSet<MouseActivities> MouseActivities { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<EmailQueue> EmailQueue { get; set; }
    }
}