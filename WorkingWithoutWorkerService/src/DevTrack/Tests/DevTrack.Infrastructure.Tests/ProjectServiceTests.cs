using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.Exceptions;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;
using ProjectUserBO = DevTrack.Infrastructure.BusinessObjects.ProjectUser;
using ProjectUserEO = DevTrack.Infrastructure.Entities.ProjectUser;

namespace DevTrack.Infrastructure.Tests
{
    public class ProjectServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IProjectService _projectService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _projectRepositoryMock = _mock.Mock<IProjectRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _projectService = _mock.Create<ProjectService>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _projectRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public void CreateProject_ProjectDoesNotExist_CreatesProject()
        {
            ProjectBO project = new ProjectBO
            {
                Title = "Data Structure and Algorithms",
                ProjectUsers = new List<ProjectUserBO>()
                {
                    new ProjectUserBO
                    {
                        ApplicationUserId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D")
                    }
                }
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Title = "Data Structure and Algorithms",
                ProjectUsers = new List<ProjectUserEO>()
                {
                    new ProjectUserEO
                    {
                        ApplicationUserId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D")
                    }
                }
            };

            _applicationtUnitOfWork.Setup(x => x.Projects).Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetCount(It.Is<Expression<Func<ProjectEO, bool>>>(y => y.Compile()(projectEntity)))).Returns(0);

            _mapperMock.Setup(x => x.Map<ProjectEO>(project))
                .Returns(projectEntity).Verifiable();

            _applicationtUnitOfWork.Setup(x => x.Save()).Verifiable();

            _projectRepositoryMock.Setup(x => x.Add(projectEntity))
                .Verifiable();

            _projectService.CreateProject(project);

            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _projectRepositoryMock.VerifyAll()
            );
        }

        [Test]
        public void CreateProject_ProjectExists_ThrowsError()
        {
            ProjectBO project = new ProjectBO
            {
                Title = "Data Structure and Algorithms",
                ProjectUsers = new List<ProjectUserBO>()
                {
                    new ProjectUserBO
                    {
                        ApplicationUserId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D")
                    }
                }
            };

            _applicationtUnitOfWork.Setup(x => x.Projects).Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetCount(It.IsAny<Expression<Func<ProjectEO, bool>>>())).Returns(1);

            Should.Throw<DuplicateException>
            (
                () => _projectService.CreateProject(project)
            );
        }

        [Test]
        public void GetProjectWhenOwner_ValidId_ReturnsProject()
        {
            var userId = Guid.Parse("4DB34D1E-6199-4FF7-7789-08DAE999D57C");
            var projectId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D");

            ProjectEO projectEntity = new ProjectEO()
            {
                Id = projectId
            };

            ProjectBO project = new ProjectBO()
            {
                Id = projectEntity.Id
            };

            _applicationtUnitOfWork.Setup(x => x.Projects).Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetById(projectId)).Returns(projectEntity);

            _projectRepositoryMock.Setup(x => x.GetCount(It.IsAny<Expression<Func<ProjectEO, bool>>>())).Returns(1);

            _mapperMock.Setup(x => x.Map<ProjectBO>(projectEntity))
                .Returns(project).Verifiable();

            var result = _projectService.GetProject(projectId, userId).Result;

            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(projectId)
            );
        }

        [Test]
        public void GetProjectWhenOwner_WhenWorkerAccessProject_ThrowsException()
        {
            var userId = Guid.Parse("4DB34D1E-6199-4FF7-7789-08DAE999D57C");
            var projectId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D");

            ProjectEO projectEntity = new ProjectEO()
            {
                Id = projectId
            };

            ProjectBO project = new ProjectBO()
            {
                Id = projectEntity.Id
            };

            _applicationtUnitOfWork.Setup(x => x.Projects).Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetById(projectId)).Returns(projectEntity);

            _projectRepositoryMock.Setup(x => x.GetCount(It.IsAny<Expression<Func<ProjectEO, bool>>>())).Returns(0);

            Should.Throw<Exception>
            (
                () => _projectService.GetProject(projectId, userId).Result
            )
            .Message
            .ShouldContain("You are not the authorized owner.");
        }

        [Test]
        public void EditProject_WhenUnAuthorizedUser_ThrowsError()
        {
            var userId = Guid.Parse("4DB34D1E-6199-4FF7-7789-08DAE999D57C");
            var projectId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D");

            ProjectBO project = new ProjectBO
            {
                Id = projectId,
                Title = "Data Structure and Algorithms",
                ProjectUsers = new List<ProjectUserBO>()
                {
                    new ProjectUserBO
                    {
                        ApplicationUserId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D")
                    }
                }
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Id = projectId,
                Title = "Data Structure",
                ProjectUsers = new List<ProjectUserEO>()
                {
                    new ProjectUserEO
                    {
                        ApplicationUserId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D")
                    }
                }
            };

            _applicationtUnitOfWork.Setup(x => x.Projects).Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetProjectDetailsById(projectId)).Returns(projectEntity);

            _projectRepositoryMock.Setup(x => x.GetCount(It.IsAny<Expression<Func<ProjectEO, bool>>>())).Returns(0);

            Should.Throw<AuthorizationException>
            (
                () => _projectService.EditProject(project, userId)
            )
            .Message
            .ShouldContain("You are not the authorized owner.");
        }

        [Test]
        public void GetProjectDetails_ValidId_ReturnsProject()
        {
            var projectId = Guid.Parse("81536C50-DBA2-4764-1270-08DAE831153D");

            ProjectEO projectEntity = new ProjectEO()
            {
                Id = projectId
            };

            ProjectBO project = new ProjectBO()
            {
                Id = projectEntity.Id
            };

            _applicationtUnitOfWork.Setup(x => x.Projects).Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetProjectDetailsById(projectId)).Returns(projectEntity);

            _mapperMock.Setup(x => x.Map<ProjectBO>(projectEntity))
                .Returns(project).Verifiable();

            var result = _projectService.GetProjectDetails(projectId).Result;

            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(projectId)
            );
        }
    }
}