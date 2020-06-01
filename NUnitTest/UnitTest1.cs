using NUnit.Framework;
using System;
using System.IO;

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

            //Act
            var result = "Hello World!";
            
            //Assert
            Assert.AreEqual(Expected, result);
              //  Assert.That(result, Is.True);
            }
        }
    }
