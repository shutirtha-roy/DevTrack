﻿using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class ActivityRepository : Repository<Activity, Guid>, IActivityRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public ActivityRepository(IApplicationDbContext context) : base((DbContext)context)
        {
            _dbContext = context;
        }

        public IEnumerable<Activity> GetActivitiesReport(Guid userId, Guid projectId, DateTime startDateTime, DateTime endDateTime)
        {
            var reports = _dbContext.Activities
                            .Where(activity => activity.ApplicationUserId == userId && activity.ProjectId == projectId)
                            .Where(activity => activity.StartTime >= startDateTime && activity.EndTime < endDateTime)
                            .Include(x => x.MouseActivity).Include(x => x.ActiveWindows)
                            .Include(x => x.ScreenCapture).Include(x => x.KeyboardActivity)
                            .Include(x => x.RunningPrograms).Include(x => x.WebcamCapture)
                            .OrderBy(x => x.EndTime);

            return reports;
        }

        public async Task<long> GetprojectLoggedTime(Project project)
        {
            return _dbContext.Activities.Where(x => x.Project == project).Select(s => s).ToList().Sum(x => x.EndTime.Ticks - x.StartTime.Ticks);
        }

        public async Task<long> GetprojectLoggedTime(Project project, Guid userId)
        {
            return _dbContext.Activities.Where(x => x.Project == project && x.ApplicationUserId == userId).Select(s => s).ToList().Sum(x => x.EndTime.Ticks - x.StartTime.Ticks);
        }

        public async Task<long> GetmonthlyLoggedTime(int month, int currentYear, Guid projectId)
        {
            return _dbContext.Activities.Where(x => x.StartTime.Month == month && x.StartTime.Year == currentYear && x.ProjectId == projectId).ToList().Sum(x => x.EndTime.Ticks - x.StartTime.Ticks);
        }

        public async Task<long> GetmonthlyLoggedTime(int month, int currentYear, Guid userId, Guid projectId)
        {
            return _dbContext.Activities.Where(x => x.StartTime.Month == month && x.StartTime.Year == currentYear && x.ApplicationUserId == userId && x.ProjectId == projectId).ToList().Sum(x => x.EndTime.Ticks - x.StartTime.Ticks);
        }
    }
}