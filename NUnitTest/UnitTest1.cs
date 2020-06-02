using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace NUnitTest
{
    [TestFixture]
    public class Tests
    {
        private const string Expected = "Hello World!";

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void GetFlightPlan_FlighInDb_ReturnFlightDb()
        {
            //Arrange
            FakeSQLCommand stub = new FakeSQLCommand();
            FakeExternalFlight fakeExternalFlight = new FakeExternalFlight();
            Mock<FakeExternalFlight> mock = new Mock<FakeExternalFlight>();
            FlightPlanController flightPlanController = new FlightPlanController(fakeExternalFlight, stub);
            string id = "";
            //Act
            var result = flightPlanController.GetFlightPlan(id);

            //Assert
            Assert.AreEqual(Expected, result);
            //  Assert.That(result, Is.True);
        }

        public FlightPlan GetExpected()
        {
            return null;
        }
    }
}