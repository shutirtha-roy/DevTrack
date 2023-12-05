using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EmailQueueBO = DevTrack.Infrastructure.BusinessObjects.EmailQueue;
using EmailQueueEO = DevTrack.Infrastructure.Entities.EmailQueue;

namespace DevTrack.Infrastructure.Tests
{
    public class EmailQueueServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IEmailQueueRepository> _emailQueueRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IEmailQueueService _emailQueueService;
        private Mock<IEmailService> _emailService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _emailQueueRepositoryMock = _mock.Mock<IEmailQueueRepository>();
            _emailService = _mock.Mock<IEmailService>();
            _mapperMock = _mock.Mock<IMapper>();

            var inMemorySettings = new Dictionary<string, string> {
                {"InvitationEmailTemplate", "invitationEmailTemplateValue"},
                {"InvitationEmailSubject", "invitationEmailSubjectValue"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            _emailQueueService = new EmailQueueService(_mapperMock.Object, _applicationtUnitOfWork.Object, configuration, _emailService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _emailQueueRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        [TestCase("32536C50-ABA2-4764-1270-08DAE831153D", "52536C50-CBA2-4764-1270-08DAE831153D", "samin@gmail.com")]
        [TestCase("754F6906-CF7B-4A7C-986E-6A8C42793C6C", "F01BAAA0-F0CB-4F5E-B867-50861EB7DE60", "abrar.bba@gmail.com")]
        public void SaveToQueueAsync_ProjectAndEmailDoesNotExist_CreatesQueue(string id, string projectId, string email)
        {
            EmailQueueBO emailQueue = new EmailQueueBO
            {
                Id = Guid.Parse(id),
                ProjectId = Guid.Parse(projectId),
                Email = email
            };

            EmailQueueEO emailQueueEntity = new EmailQueueEO
            {
                Id = Guid.Parse(id),
                ProjectId = Guid.Parse(projectId),
                Email = email
            };

            _applicationtUnitOfWork.Setup(x => x.EmailQueue).Returns(_emailQueueRepositoryMock.Object);

            _mapperMock.Setup(x => x.Map<EmailQueueEO>(emailQueue))
                .Returns(emailQueueEntity).Verifiable();

            _emailQueueRepositoryMock.Setup(x => x.Add(emailQueueEntity))
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.Save()).Verifiable();

            _emailQueueService.SaveToQueueAsync(emailQueue);

            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _emailQueueRepositoryMock.VerifyAll()
            );
        }
    }
}