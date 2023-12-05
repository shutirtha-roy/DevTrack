using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq.Expressions;
using InvitationBO = DevTrack.Infrastructure.BusinessObjects.Invitation;
using InvitationEO = DevTrack.Infrastructure.Entities.Invitation;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;

namespace DevTrack.Infrastructure.Tests
{
    public class InvitationServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IInvitationRepository> _invitationRepositoryMock;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IInvitationService _invitationService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _invitationRepositoryMock = _mock.Mock<IInvitationRepository>();
            _projectRepositoryMock = _mock.Mock<IProjectRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _invitationService = _mock.Create<InvitationService>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _invitationRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public void SaveToInvitationAsync_ValidInvitation_SendsInvitation()
        {
            InvitationBO invitation = new InvitationBO
            {
                Id = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                ProjectId = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                Email = "samin@gmail.com"
            };

            InvitationEO invitationEntity = new InvitationEO
            {
                Id = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                ProjectId = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                Email = "samin@gmail.com"
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Id = invitationEntity.ProjectId
            };

            _applicationtUnitOfWork.Setup(x => x.Invitations).Returns(_invitationRepositoryMock.Object);

            _mapperMock.Setup(x => x.Map<InvitationEO>(invitation))
                .Returns(invitationEntity).Verifiable();

            _invitationRepositoryMock.Setup(x => x.GetPendingInvitationCountByEmail(invitationEntity.Email, invitationEntity.ProjectId)).Returns(0);

            _projectRepositoryMock.Setup(x => x.GetProjectDetailsById(invitationEntity.ProjectId)).Returns(projectEntity);

            _applicationtUnitOfWork.Setup(x => x.Projects).Returns(_projectRepositoryMock.Object);

            _invitationRepositoryMock.Setup(x => x.Add(invitationEntity))
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.Save()).Verifiable();

            _invitationService.SaveToInvitationAsync(invitation);
            
            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _invitationRepositoryMock.VerifyAll(),
                () => _projectRepositoryMock.VerifyAll()
            );
        }

        [Test]
        public void SaveToInvitationAsync_InvalidInvitation_DoesNotSendInvitation()
        {
            InvitationBO invitation = new InvitationBO
            {
                Id = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                ProjectId = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                Email = "samin@gmail.com"
            };

            InvitationEO invitationEntity = new InvitationEO
            {
                Id = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                ProjectId = Guid.Parse("92536C50-DBA2-4764-1270-08DAE831153D"),
                Email = "samin@gmail.com"
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Id = invitationEntity.ProjectId
            };

            _applicationtUnitOfWork.Setup(x => x.Invitations).Returns(_invitationRepositoryMock.Object);

            _mapperMock.Setup(x => x.Map<InvitationEO>(invitation))
                .Returns(invitationEntity).Verifiable();

            _invitationRepositoryMock.Setup(x => x.GetPendingInvitationCountByEmail(invitationEntity.Email, invitationEntity.ProjectId)).Returns(0);

            _projectRepositoryMock.Setup(x => x.GetProjectDetailsById(invitationEntity.ProjectId)).Returns(projectEntity);

            _invitationRepositoryMock.Setup(x => x.Add(invitationEntity))
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.Save()).Verifiable();

            _invitationService.SaveToInvitationAsync(invitation);

            _applicationtUnitOfWork.Verify(x => x.Save(), Times.Never);
            _invitationRepositoryMock.Verify(x => x.Add(invitationEntity), Times.Never);
            _projectRepositoryMock.Verify(x => x.GetById(invitationEntity.ProjectId), Times.Never);
        }
    }
}