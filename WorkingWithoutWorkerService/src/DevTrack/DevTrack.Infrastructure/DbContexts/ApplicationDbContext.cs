using DevTrack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>, IApplicationDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ApplicationDbContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    b => b.MigrationsAssembly(_migrationAssemblyName)
                );
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>().HasKey(c => new { c.ApplicationUserId, c.ProjectId });

            modelBuilder.Entity<ProjectUser>()
               .HasOne(x => x.Project)
               .WithMany(x => x.ProjectUsers)
               .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.UserProjects)
                .HasForeignKey(x => x.ApplicationUserId);

            modelBuilder.Entity<Activity>()
                .HasOne<ScreenCapture>(s => s.ScreenCapture)
                .WithOne(ad => ad.Activity)
                .HasForeignKey<ScreenCapture>(ad => ad.ActivityId);

            modelBuilder.Entity<Activity>()
               .HasOne<WebcamCapture>(s => s.WebcamCapture)
               .WithOne(ad => ad.Activity)
               .HasForeignKey<WebcamCapture>(ad => ad.ActivityId);

            modelBuilder.Entity<Activity>()
               .HasMany<ActiveWindows>(s => s.ActiveWindows)
               .WithOne(ad => ad.Activity)
               .HasForeignKey(ad => ad.ActivityId);

            modelBuilder.Entity<Activity>()
               .HasMany<RunningProgram>(s => s.RunningPrograms)
               .WithOne(ad => ad.Activity)
               .HasForeignKey(ad => ad.ActivityId);

            modelBuilder.Entity<Activity>()
               .HasOne<MouseActivities>(s => s.MouseActivity)
               .WithOne(ad => ad.Activity)
               .HasForeignKey<MouseActivities>(ad => ad.ActivityId);

            modelBuilder.Entity<Activity>()
               .HasOne<KeyboardActivities>(s => s.KeyboardActivity)
               .WithOne(ad => ad.Activity)
               .HasForeignKey<KeyboardActivities>(ad => ad.ActivityId);

            base.OnModelCreating(modelBuilder);
        }

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