using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;

namespace DevTrack.Web.Areas.App.Models
{
    public class ActivityRequestModel : BaseModel
    {
        public Guid ActivityId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOnline { get; set; }
        public ScreenCaptureModel? ScreenCapture { get; set; }
        public WebcamCaptureModel? WebcamCapture { get; set; }
        public IList<RunningProgramModel>? RunningProgram { get; set; }
        public IList<ActiveWindowsModel>? ActiveWindows { get; set; }
        public KeyboardActivitiesModel? KeyboardActivity { get; set; }
        public MouseActivitiesModel? MouseActivity { get; set; }

        private IActivityService? _activityService;
        private IMapper? _mapper;

        public ActivityRequestModel() : base()
        {

        }

        public ActivityRequestModel(IActivityService activityService, IMapper mapper)
        {
            _activityService = activityService;
            _mapper = mapper;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _activityService = _scope.Resolve<IActivityService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        internal async Task<Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<Activity> reportSlot)?>>> 
            GetActivitiesReport(Guid userId, Guid projectId, DateTime startDateTime, DateTime endDateTime)
        {
            var reportActivity = await _activityService.GetActivitiesReport(userId, projectId, startDateTime, endDateTime);

            return reportActivity;  
        }
    }
}