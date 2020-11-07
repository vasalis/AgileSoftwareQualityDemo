using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AwesomeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Container _myDb;

        public StudentController(ILogger<WeatherForecastController> logger, Container aDbContainer)
        {
            _logger = logger;
            _myDb = aDbContainer;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
            try
            {
                List<StudentEntity> lResults = new List<StudentEntity>();

                QueryDefinition queryDefinition = new QueryDefinition("select * from Students");

                using (FeedIterator<StudentEntity> feedIterator = this._myDb.GetItemQueryIterator<StudentEntity>(
                    queryDefinition,
                    null))
                {
                    while (feedIterator.HasMoreResults)
                    {
                        foreach (var item in await feedIterator.ReadNextAsync())
                        {
                            lResults.Add(item);
                        }
                    }

                    return lResults;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not Get Students. Exception thrown: {ex.Message}");                
            }

            return null;
        }

        // GET: api/Student/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IEnumerable<StudentEntity>> Get(string id)
        {
            try
            {

                this.DoCompute();

                List<StudentEntity> lResults = new List<StudentEntity>();

                QueryDefinition queryDefinition = new QueryDefinition($"select * from Students");

                using (FeedIterator<StudentEntity> feedIterator = this._myDb.GetItemQueryIterator<StudentEntity>(
                    queryDefinition,
                    null,
                    new QueryRequestOptions() { PartitionKey = new PartitionKey(id) }))
                {
                    while (feedIterator.HasMoreResults)
                    {
                        foreach (var item in await feedIterator.ReadNextAsync())
                        {
                            lResults.Add(item);
                        }
                    }

                    return lResults;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not Get Student with id: {id}. Exception thrown: {ex.Message}");
            }

            return null;
        }

        // POST: api/Student
        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            var lStudent = JsonConvert.DeserializeObject<StudentEntity>(value);

            await _myDb.UpsertItemAsync(lStudent);
        }

        private void DoCompute()
        {
            double result = 0;
            for (var i = Math.Pow(6, 8); i >= 0; i--)
            {
                result += Math.Atan(i) * Math.Tan(i);
            }
        }
    }
}
