using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using ActivityEO = DevTrack.Infrastructure.Entities.Activity;
using ScreenCaptureBO = DevTrack.Infrastructure.BusinessObjects.ScreenCapture;
using ScreenCaptureEO = DevTrack.Infrastructure.Entities.ScreenCapture;
using WebcamCaptureBO = DevTrack.Infrastructure.BusinessObjects.WebcamCapture;
using WebcamCaptureEO = DevTrack.Infrastructure.Entities.WebcamCapture;
using RunningProgramsBO = DevTrack.Infrastructure.BusinessObjects.RunningProgram;
using RunningProgramsEO = DevTrack.Infrastructure.Entities.RunningProgram;
using ActiveWindowsBO = DevTrack.Infrastructure.BusinessObjects.ActiveWindows;
using ActiveWindowsEO = DevTrack.Infrastructure.Entities.ActiveWindows;
using KeyboardActivityBO = DevTrack.Infrastructure.BusinessObjects.KeyboardActivities;
using KeyboardActivityEO = DevTrack.Infrastructure.Entities.KeyboardActivities;
using MouseActivityBO = DevTrack.Infrastructure.BusinessObjects.MouseActivities;
using MouseActivityEO = DevTrack.Infrastructure.Entities.MouseActivities;
using Shouldly;
using DevTrack.Infrastructure.Enum;
using System.Threading.Tasks;

namespace DevTrack.Infrastructure.Tests
{
    public class ActivityServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IImageService> _imageServiceMock;
        private Mock<IActivityRepository> _activityRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IActivityService _activityService;
        private string _screenCaptureBase64;
        private string _webCaptureBase64;
        private string _imageNameWithExtension;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _imageServiceMock = _mock.Mock<IImageService>();
            _activityRepositoryMock = _mock.Mock<IActivityRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _activityService = _mock.Create<ActivityService>();

            _screenCaptureBase64 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            _screenCaptureBase64 = _screenCaptureBase64.Replace("=", "");
            _screenCaptureBase64 = _screenCaptureBase64.Replace("+", "");

            _webCaptureBase64 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            _webCaptureBase64 = _webCaptureBase64.Replace("=", "");
            _webCaptureBase64 = _webCaptureBase64.Replace("+", "");

            _imageNameWithExtension = "sometime.png";
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _imageServiceMock.Reset();
            _activityRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        private ActivityBO GetActivityBO()
        {
            return new ActivityBO
            {
                ActivityId = Guid.Parse("0636E901-D273-4F89-82AD-00060F3290AB"),
                UserId = Guid.Parse("9CDA3E68-C99D-42C1-8815-08DAF1714E6C"),
                ProjectId = Guid.Parse("866A75B3-19F9-4426-85FB-08DAF171602F"),
                Description = "Project Demonstration",
                StartTime = new DateTime(2023, 1, 13, 7, 0, 0),
                EndTime = new DateTime(2023, 1, 13, 7, 2, 0),
                IsOnline = true,
                ScreenCapture = new ScreenCaptureBO
                {
                    Image = _screenCaptureBase64
                },
                WebcamCapture = new WebcamCaptureBO
                {
                    Image = _webCaptureBase64
                },
                RunningPrograms = new List<RunningProgramsBO>
                {
                    new RunningProgramsBO
                    {
                        MainWindowTitle = "Mail",
                        ProcessName = "ApplicationFrameHost"
                    }
                },
                ActiveWindows = new List<ActiveWindowsBO>
                {
                    new ActiveWindowsBO
                    {
                        ProcessName = "chrome",
                        MainWindowTitle = "Youtube - Google Chrome"
                    }
                },
                KeyboardActivity = new KeyboardActivityBO
                {
                    TotalHits = 1,
                    KeyCounts = "{a : 1}"
                },
                MouseActivity = new MouseActivityBO
                {
                    TotalHits = 20
                }
            };
        }

        private ActivityEO GetActivityEO()
        {
            return new ActivityEO
            {
                Id = Guid.Parse("0636E901-D273-4F89-82AD-00060F3290AB"),
                ApplicationUserId = Guid.Parse("9CDA3E68-C99D-42C1-8815-08DAF1714E6C"),
                ProjectId = Guid.Parse("866A75B3-19F9-4426-85FB-08DAF171602F"),
                Description = "Project Demonstration",
                StartTime = new DateTime(2023, 1, 13, 7, 0, 0),
                EndTime = new DateTime(2023, 1, 13, 7, 2, 0),
                IsOnline = true,
                ScreenCapture = new ScreenCaptureEO
                {
                    Image = _screenCaptureBase64
                },
                WebcamCapture = new WebcamCaptureEO
                {
                    Image = _webCaptureBase64
                },
                RunningPrograms = new List<RunningProgramsEO>
                {
                    new RunningProgramsEO
                    {
                        MainWindowTitle = "Mail",
                        ProcessName = "ApplicationFrameHost"
                    }
                },
                ActiveWindows = new List<ActiveWindowsEO>
                {
                    new ActiveWindowsEO
                    {
                        ProcessName = "chrome",
                        MainWindowTitle = "Youtube - Google Chrome"
                    }
                },
                KeyboardActivity = new KeyboardActivityEO
                {
                    TotalHits = 1,
                    KeyCounts = "{a : 1}"
                },
                MouseActivity = new MouseActivityEO
                {
                    TotalHits = 20
                }
            };
        }

        private async Task<string> GetImageTaskValue()
        {
            return _imageNameWithExtension;
        }

        [Test]
        public void CreateActivity_WhenActivityIsPassed_CreatesActivity()
        {
            ActivityBO activity = GetActivityBO();

            ActivityEO activityEntity = GetActivityEO();

            _mapperMock.Setup(x => x.Map<ActivityEO>(activity))
                .Returns(activityEntity).Verifiable();

            _imageServiceMock.Setup(x => x.ConvertBase64StringToImage(It.IsAny<string>(), It.IsAny<ImageType>())).Returns(GetImageTaskValue());

            _applicationtUnitOfWork.Setup(x => x.Activities).Returns(_activityRepositoryMock.Object);

            _activityRepositoryMock.Setup(x => x.Add(activityEntity)).Verifiable();

            _applicationtUnitOfWork.Setup(x => x.Save()).Verifiable();

            _activityService.CreateActivity(activity);

            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _activityRepositoryMock.VerifyAll()
            );
        }
    }
}