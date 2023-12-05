using Autofac.Extras.Moq;
using DevTrack.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevTrack.Infrastructure.Tests
{
    public class TimeServiceTests
    {
        private AutoMock _mock;
        private ITimeService _timeService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _timeService = _mock.Create<TimeService>();
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        private DateTime TrimSeconds(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);
        }

        [Test]
        public void GetLocalTimeZoneRegion_WhenCalled_ReturnsTimeZone()
        {
            var timeZone = _timeService.GetLocalTimeZoneRegion().Result;
            var expected = "(UTC+06:00) Bangladesh Standard Time";

            Assert.AreEqual(expected, timeZone);
        }

        [Test]
        [TestCase("(UTC+11:00) Magadan Standard Time", 11)]
        [TestCase("(UTC-03:00) SA Eastern Standard Time", -3)]
        public void GetTimeFromTimeZone_WhenValid_ReturnsTime(string timeZoneWithRegion, int hour)
        {
            var time = TrimSeconds(_timeService.GetTimeFromTimeZone(timeZoneWithRegion).Result);
            var expectedTime = TrimSeconds(DateTime.UtcNow.AddHours(hour));

            Assert.AreEqual(expectedTime, time);
        }

        [Test]
        public void GetAllTimeZone_WhenCalled_ReturnsAllTimeZones()
        {
            var allZones = _timeService.GetAllTimeZone().Result;
            var totalAllZones = new List<SelectListItem>(allZones).Count;
            var expected = 0;

            Assert.AreNotEqual(expected, totalAllZones);
        }
    }
}
