using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WorldCountryDataMicroService.Models;
using WorldCountryDataMicroService.RabbitMQ;
using Newtonsoft.Json;

namespace WorldCountryDataMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorldCountryDummyController : ControllerBase
    {
        IRabbitMQPublisher _rabbitmqPublisher;

        public WorldCountryDummyController(IRabbitMQPublisher rabbitmqPublisher)
        {
            _rabbitmqPublisher = rabbitmqPublisher;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<WorldCountryDummyModel> Get()
        {
            WorldCountryDummyModel model = new WorldCountryDummyModel();
            IEnumerable<WorldCountryDummyModel> modelList = model.GetDumyList();

            string jsonList = JsonConvert.SerializeObject(modelList);
            _rabbitmqPublisher.SendMessage("world_dummy", jsonList);

            return modelList;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<WorldCountryDummyModel> Get(int id)
        {
            WorldCountryDummyModel model = new WorldCountryDummyModel();
            IEnumerable<WorldCountryDummyModel> modelList = model.GetDumyList();

            return modelList.FirstOrDefault(m => m.Number == id);
        }

        // POST api/values
        [HttpPost]
        public IEnumerable<WorldCountryDummyModel> Post([FromBody] WorldCountryDummyModel value)
        {
            WorldCountryDummyModel model = new WorldCountryDummyModel();
            IEnumerable<WorldCountryDummyModel> modelList = model.GetDumyList();
            List<WorldCountryDummyModel> MyReturnList = new List<WorldCountryDummyModel>();
            MyReturnList.AddRange(modelList);
            MyReturnList.Insert(0, value);

            return MyReturnList;

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IEnumerable<WorldCountryDummyModel> Put(int id, [FromBody] WorldCountryDummyModel value)
        {
            WorldCountryDummyModel model = new WorldCountryDummyModel();
            IEnumerable<WorldCountryDummyModel> modelList = model.GetDumyList();

            WorldCountryDummyModel modelToUpdate = modelList.FirstOrDefault(m => m.Number == id);

            List<WorldCountryDummyModel> ListToReturn = new List<WorldCountryDummyModel>();
            ListToReturn.Add(modelToUpdate);
            ListToReturn.Add(value);

            return ListToReturn;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IEnumerable<WorldCountryDummyModel> Delete(int id)
        {
            WorldCountryDummyModel model = new WorldCountryDummyModel();
            IEnumerable<WorldCountryDummyModel> modelList = model.GetDumyList();


            IEnumerable<WorldCountryDummyModel> newReturnList = modelList.Where(m => m.Number != id);

            return newReturnList;
        }
    }
}
