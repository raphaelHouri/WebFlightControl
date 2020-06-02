using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NUnitTest
{
    [TestFixture]
    public class Tests
    {

        public FlightPlan try1(FlightPlan blas)
            {

            return null;
            }
        
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
        public Task<FlightPlan> FakeExternalFlight()
        {
            return Task.Run(() =>
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
            });

        }
       

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public async Task GetFlightPlan_FlighInDb_ReturnFlightDbAsync()
        {
            //Arrange
            string id = "test123";
            FlightPlan Expected = GetFlightExpected();
            StubSQLCommand stub = new StubSQLCommand();
            //FakeExternalFlight fakeExternalFlight = new FakeExternalFlight();
            Mock<IExternalFlights> mock = new Mock<IExternalFlights>();
            //FlightPlan fake = FakeExternalFlight();
            mock.Setup(x => x.GetExternalFlightById(id))
                .Returns(FakeExternalFlight());
            //injection with the fake data
            FlightPlanController flightPlanController = new FlightPlanController(mock.Object, stub);
           
            //Act
            var result = await flightPlanController.GetFlightPlan(id);
            ActionResult mod = result.Result;
            var ok = mod as OkObjectResult;
            FlightPlan resultFlight = (FlightPlan)ok.Value;
           // var flight = result.Value;
             //var flight = result.Result;
             //var a=flight.();
           

            //Assert
            var object1Json = JsonConvert.SerializeObject(Expected);
            var object2Json = JsonConvert.SerializeObject(resultFlight);
           Assert.AreEqual(object1Json, object2Json);
         //   Assert.AreEqual(Expected.C );
            mock.Verify(x=> x.GetExternalFlightById(id),Times.Never);
        }

        
    }
}