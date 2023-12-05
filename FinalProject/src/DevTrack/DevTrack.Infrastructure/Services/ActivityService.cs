using AutoMapper;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using ActivityEO = DevTrack.Infrastructure.Entities.Activity;
using DevTrack.Infrastructure.UnitOfWorks;
using DevTrack.Infrastructure.Enum;

namespace DevTrack.Infrastructure.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IImageService _imageService;
        private readonly IReportService _reportService;
        private readonly ITimeService _timeService;
        private readonly IMapper _mapper;

        public ActivityService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork
            , IImageService imageService, IReportService reportService, ITimeService timeService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _imageService = imageService;
            _reportService = reportService;
            _timeService = timeService;
        }

        public async Task CreateActivity(ActivityBO activity)
        {
            try
            {
                var activityEntity = _mapper.Map<ActivityEO>(activity);

                if (activityEntity.WebcamCapture != null && activityEntity.WebcamCapture.Image != null)
                    activityEntity.WebcamCapture.Image = await _imageService.ConvertBase64StringToImage(
                        activity.WebcamCapture.Image, ImageType.WebcamCapture);

                if (activityEntity.ScreenCapture != null && activityEntity.ScreenCapture.Image != null)
                    activityEntity.ScreenCapture.Image = await _imageService.ConvertBase64StringToImage(
                        activity.ScreenCapture.Image, ImageType.ScreenCapture);

                _applicationUnitOfWork.Activities.Add(activityEntity);
                _applicationUnitOfWork.Save();
            }
            catch (Exception)
            {
                throw new Exception("Activity can't be save!");
            }
        }

        public async Task<Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<ActivityBO> reportSlot)?>>> 
            GetActivitiesReport(Guid userId, Guid projectId, DateTime startTime, DateTime endTime)
        {
            endTime = endTime.AddDays(1);

            var reports = _applicationUnitOfWork.Activities.GetActivitiesReport(userId, projectId, startTime, endTime).ToList();

            var dates = await _timeService.GetAllSelectedDates(startTime, endTime);

            if(dates.Length > 7)
            {
                throw new Exception("You cannot select more than 7 days.");
            }

            var dateAccessedPair = await InitializeDateExistsPair(dates);

            bool isAccessed = false;

            var dataSlot = new Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<ActivityBO> reportSlot)?>>();
            var allSelectedReportSlots = new Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<ActivityBO> reportSlot)?>>();

            foreach (var report in reports)
            {
                ActivityBO projectBO = _mapper.Map<ActivityBO>(report);
                await projectBO.RoundOffMinutes();
                var date = DateOnly.FromDateTime(projectBO.StartTime);

                if (dateAccessedPair[date] == false)
                {
                    if(isAccessed == false)
                    {
                        dataSlot = await _reportService.GetReportValuesOfDates<ActivityBO>(date);
                        isAccessed = true;
                    }
                    else
                    {
                        allSelectedReportSlots.Add(dataSlot.First().Key, dataSlot.First().Value);
                        dataSlot = await _reportService.GetReportValuesOfDates<ActivityBO>(date);
                    }
                }

                dateAccessedPair[date] = true;
                dataSlot[date][(TimeOnly.FromDateTime(projectBO.StartTime), TimeOnly.FromDateTime(projectBO.EndTime))].Value.reportSlot.Add(projectBO);
            }

            allSelectedReportSlots.Add(dataSlot.First().Key, dataSlot.First().Value);

            if (allSelectedReportSlots.Count == 0)
                throw new Exception("No Report Available");

            return allSelectedReportSlots;
        }

        private async Task<Dictionary<DateOnly, bool>> InitializeDateExistsPair(DateOnly[] dates)
        {
            var dateAccessedPair = new Dictionary<DateOnly, bool>();

            foreach (var date in dates)
            {
                dateAccessedPair[date] = false;
            }

            return dateAccessedPair;
        }
    }
}