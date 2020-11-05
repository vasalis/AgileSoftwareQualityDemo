using AwesomeApp;
using AwesomeApp.Controllers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeAppUnitTests
{
    public class StudentControllerTests
    {
        private IConfiguration mConfig;

        [SetUp]
        public void Setup()
        {
            mConfig = new ConfigurationBuilder()
              .AddJsonFile("appsettings.Test.json").Build();
        }

        /// <summary>
        /// This is an integration test
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestCreateAndRead()
        {
            // Get a database connection - a Cosmos Db Container in this case. This could be for example another db connection string
            // For demo purpuse this is hardcoded - it should be dynamic and set by the CI pipeline
            var lDb = GetContainer();

            // Get an instance of the Student Controller
            // We need to pass a Cosmos Db Container Object on the constructor
            StudentController lController = new StudentController(null, lDb);


            // Instantiate a new Student Object
            string lId = Guid.NewGuid().ToString();

            StudentEntity lNewStudent = new StudentEntity();
            lNewStudent.FirstName = $"New Student First name - {lId}";
            lNewStudent.LastName = $"New Student Last name - {lId}";
            lNewStudent.Id = lId;

            // Post it - this will create an object in the Db
            await lController.Post(JsonConvert.SerializeObject(lNewStudent));

            // Get created student from DB
            var lStudents = await lController.Get(lNewStudent.Id);

            // Test it :)
            Assert.AreEqual(lNewStudent.Id, lStudents.ToList()[0].Id);            
        }

        private Container GetContainer()
        {
            var lConnectionString = mConfig["CosmosConnectionString"];
            var lCosmosDbName = mConfig["CosmosDbName"];
            var lCosmosDbContainerName = mConfig["CosmosDbContainerName"];
            var lCosmosDbPartionKey = mConfig["CosmosDbPartitionKey"];

            var lClient = new CosmosClient(lConnectionString, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Direct
            });

            lClient.CreateDatabaseIfNotExistsAsync(lCosmosDbName).Wait();
            var lDb = lClient.GetDatabase(lCosmosDbName);
            lDb.CreateContainerIfNotExistsAsync(lCosmosDbContainerName, lCosmosDbPartionKey).Wait();

            return lDb.GetContainer(lCosmosDbContainerName);
        }
    }
}