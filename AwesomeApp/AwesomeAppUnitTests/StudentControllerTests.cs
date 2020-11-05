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

        [Test]
        public async Task TestCreateAndRead()
        {
            var lDb = GetContainer();

            StudentController lController = new StudentController(null, lDb);

            StudentEntity lNewStudent = new StudentEntity();
            lNewStudent.FirstName = "New Student First name";
            lNewStudent.LastName = "New Student Last name";
            lNewStudent.Id = Guid.NewGuid().ToString();


            await lController.Post(JsonConvert.SerializeObject(lNewStudent));

            // Get created student from DB
            var lStudents = await lController.Get(lNewStudent.Id);

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