using AwesomeApp.Controllers;
using NUnit.Framework;
using System.Linq;

namespace AwesomeAppUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGet()
        {
            WeatherForecastController lController = new WeatherForecastController(null);

            var lResult = lController.Get();

            Assert.AreEqual(6, lResult.ToList().Count);            
        }
    }
}