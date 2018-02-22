using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Actor1.Interfaces;
using Microsoft.ServiceFabric.Actors.Client;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class PIDController : Controller
    {
        [HttpGet]
        [Route ("{id}/{measuredValue}/{setPoint}/{Kp}/{Ki}/{Kd}")]
        public async Task<string> Get(int id, double measuredValue, double setPoint, double Kp, double Ki, double Kd)
        {
            ActorId actorId = new ActorId(id);
            var actor1 = ActorProxy.Create<IActor1>(actorId, new Uri("fabric:/Application9/Actor1ActorService"));
            double response = await actor1.RunAsync(param1, x, param2, y);
            return response.ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
