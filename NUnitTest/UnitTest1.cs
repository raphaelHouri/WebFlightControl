using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace NUnitTest
{
    [TestFixture]
    public class Tests
    {

        public FlightPlan GetFlightExpected()
        {
            int passengers = 2;
            string company_name = "ISRAIR";
            string time = "2020-06-01T12:32:00Z";
            DateTime dt = DateTime.Parse(time);
            dt = TimeZoneInfo.ConvertTimeToUtc(dt);
            InitialLocation initial_location = new InitialLocation(34.957610, 29.555631, dt);
            List<Segment> segments = new List<Segment>(){
             new Segment(35.211514,31.769399,10000)
            };
            FlightPlan flightPlanExpected = new FlightPlan(passengers, company_name, initial_location, segments);
            return flightPlanExpected;
        }

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void GetFlightPlan_FlighInDb_ReturnFlightDb()
        {
            //Arrange
            FlightPlan Expected = GetFlightExpected();
            StubSQLCommand stub = new StubSQLCommand();
            //FakeExternalFlight fakeExternalFlight = new FakeExternalFlight();
            Mock<IExternalFlights> mock = new Mock<IExternalFlights>();
            //injection with the fake data
            FlightPlanController flightPlanController = new FlightPlanController(mock.Object, stub);
            string id = "test123";
            
            //Act
            var result = flightPlanController.GetFlightPlan(id);

            //Assert
            Assert.AreEqual(Expected, result);
            mock.Verify(x=> x.GetExternalFlightById(id),Times.Never);
        }

        
    }
}